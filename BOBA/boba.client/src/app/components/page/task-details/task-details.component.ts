import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { response } from 'express';
import { ApiService, ChoiceSummaryDto, FormFieldDto, MoveTaskRequest, TaskFieldDto, TaskFlowSummaryDto, TaskSummaryDto } from '../../../services/api-service.service';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms'
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

@Component({
    selector: 'app-task-details',
    templateUrl: './task-details.component.html',
    styleUrl: './task-details.component.css',
    standalone: false
})
export class TaskDetailsComponent implements OnInit{
  taskId!: string;
  task!: TaskSummaryDto;
  taskflow!: TaskFlowSummaryDto;
  choiceIds!: string[];
  choices!: ChoiceSummaryDto[];
  selectedChoiceId: string | null = null;
  selectedChoice: ChoiceSummaryDto | null = null;
  nextStateName: string | null = null;
  showConfirmDialog: boolean = false;

  dynamicForm!: FormGroup;
  mergedFields: any[] = [];

  constructor(
    private apiService: ApiService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  async loadData(): Promise<void> {
    await this.loadRoute();
    await this.loadTask();

    if(!this.task.currentStateIsFinal) {
        await this.loadTaskFlow();
        await this.loadChoices();
        await this.buildDynamicForm();  // <-- new function
    }
}

  loadRoute(): Promise<void> {
    return new Promise((resolve) => {
      this.route.paramMap.subscribe(params => {
        const id = params.get('id');
        if (id !== null) {
          this.taskId = id;
        }
        resolve();
      });
    });
  }

  loadTask(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.apiService.task_GetTaskById(this.taskId).subscribe({
        next: (task) => {
          this.task = task;
          console.log(this.task);
          resolve();
        },
        error: (err) => {
          console.error('Error loading task:', err);
          reject(err);
        }
      });
    });
  }

  loadTaskFlow(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.apiService.taskFlow_GetTaskFlow(this.taskId).subscribe({
        next: (taskflow) => {
          this.taskflow = taskflow;
          console.log(this.taskflow);
          resolve();
        },
        error: (err) => {
          console.error('Error loading taskflow:', err);
          reject(err);
        }
      });
    });
  }

  async buildDynamicForm(): Promise<void> {
    if (!this.taskflow?.formFields) return;

    const fieldIds = this.taskflow.formFields.flatMap(section =>
        section.fields?.map(f => f.fieldId!) ?? []
    );

    if (!fieldIds.length) return;

    const taskFields: TaskFieldDto[] = (await this.apiService.form_GetTaskFieldsByName(fieldIds).toPromise()) || [];
    const taskFieldIds: string[] = taskFields.map(tf => tf.id!).filter(id => !!id);
    const savedFields: FormFieldDto[] = (await this.apiService.form_GetSavedFieldsForTask(this.taskId, taskFieldIds).toPromise()) || [];


    this.mergedFields = [];

    for (const section of this.taskflow.formFields) {
        const mergedSection: any = {
            layout: section.layout,
            fields: []
        };

        for (const field of section.fields ?? []) {
            const metadata = taskFields.find(tf => tf.name === field.fieldId);
            console.log(metadata);
            const saved = savedFields.find(sf => sf.modelId === metadata?.id);
            console.log(saved);

            const mergedField = {
                ...field,
                ...metadata,
                value: saved?.value ?? '',
            };

            mergedSection.fields.push(mergedField);
        }

        this.mergedFields.push(mergedSection);
    }

    const group: { [key: string]: FormControl } = {};

    this.mergedFields.forEach(section => {
      section.fields.forEach((f: any) => {
        const validators = [];

        if (f.required) {
          validators.push(this.conditionalRequired());
        }

        if (f.validation === 'email') {
          validators.push(Validators.email);
        }

        group[f.fieldId] = new FormControl(
          { value: f.value ?? '', disabled: f.disabled },
          validators
        );
      });
    });

    this.dynamicForm = new FormGroup(group);

}

  conditionalRequired(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (value === null || value === undefined || value === '') {
      return null;
    }
    return value ? null : { required: true };
  };
}


  loadChoices(): Promise<void> {
    return new Promise((resolve, reject) => {
      try{
        this.choiceIds = [];
        if (this.taskflow?.nextState) {
          for (const item of this.taskflow.nextState) {
            if (item.choiceId) {
              this.choiceIds.push(item.choiceId);
            }
          }
        }
        this.apiService.taskFlow_GetChoices(this.choiceIds).subscribe({
          next: (choices) => {
            this.choices = choices;
            console.log(this.choices);
            resolve();
          },
          error: (err) => {
            console.log('Error loading choices:', err)
            reject(err);
          }
        });
      } catch(error) {
        reject(error);
      }
    });
  }

  onChoiceSelected(choice: ChoiceSummaryDto): void {
    this.selectedChoice = choice;
    if (this.taskflow?.nextState) {
      const nextState = this.taskflow.nextState.find(state => state.choiceId === choice.id);
      if (nextState && this.selectedChoiceId) {
        this.apiService.taskFlow_GetTaskStateName(this.selectedChoiceId).subscribe({
          next: (stateName) => {
            this.nextStateName = stateName;
            console.log('Next state: ' + stateName);
          },
          error: (err) => {
            console.log('Error loading statename:', err)
          }
        });
      }
    }
  }

  openConfirmationDialog(): void {
    this.showConfirmDialog = true;
  }

  cancelConfirmation(): void {
    this.showConfirmDialog = false;
  }

  submitTask(): void {
    if (this.dynamicForm && !this.dynamicForm.valid) {
        console.log('Form is invalid');
        Object.entries(this.dynamicForm.controls).forEach(([name, control]) => {
          console.log(name, control.value, control.errors);
          if (control.invalid) console.log(name, control.value, control.errors);
        });
        this.dynamicForm.markAllAsTouched();
        return;
    }

    const formFieldDtos: FormFieldDto[] = [];

    this.mergedFields.forEach(section => {
        section.fields.forEach((f: any) => {
            const dto = new FormFieldDto();   // <-- instantiate the class
            dto.modelId = f.fieldId;
            dto.taskId = this.taskId;
            dto.value = this.dynamicForm.get(f.fieldId)?.value;
            formFieldDtos.push(dto);
        });
    });

    this.apiService.form_SaveFormFields(this.taskId, formFieldDtos).subscribe({
        next: () => {
            console.log('Form saved successfully');

            const request = new MoveTaskRequest({
                choiceId: this.selectedChoiceId!,
                taskId: this.taskId!
            });

            this.apiService.task_UpdateTask(this.taskId!, request).subscribe(
                (response) => {
                    console.log('Task moved to next state successfully', response);
                    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
                        this.router.navigate([`/task-details/${this.taskId}`]);
                    });
                },
                (error) => {
                    console.error('Error moving task', error);
                }
            );

            this.showConfirmDialog = false;
        },
        error: err => {
            console.error('Error saving form:', err);
        }
    });
}

}
