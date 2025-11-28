import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { response } from 'express';
import { ApiService, ChoiceSummaryDto, FormDocumentDto, FormFieldDto, MoveTaskRequest, TaskDocTypeDto, TaskFieldDto, TaskFlowSummaryDto, TaskSummaryDto } from '../../../services/api-service.service';
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

  docTypes: TaskDocTypeDto[] = [];
  documents: FormDocumentDto[] = [];
  dragOverDocType: string | null = null;

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
    await this.loadTaskFlow();
    await this.buildDynamicForm();
    await this.loadDocuments();

    if(!this.task.currentStateIsFinal) {
        await this.loadChoices();
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
          console.log('cica');
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

        if (f.validation) {
          if (f.validation === 'email') {
            validators.push(Validators.email);
          } else {
            validators.push(Validators.pattern(f.validation));
          }
        }

        group[f.fieldId] = new FormControl(
          {
            value: f.value === null || f.value === undefined ? '' : f.value,
            disabled: f.disabled
          },
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
        return { required: true };
      }
      return null;
    };
  }

  isFieldRequired(fieldId: string): boolean {
    const control = this.dynamicForm?.get(fieldId);
    if (!control) return false;

    const validator = control.validator;
    if (!validator) return false;

    const emptyControl = new FormControl('');
    const validation = validator(emptyControl);
    return validation !== null && validation['required'] === true;
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

  async loadDocuments(): Promise<void> {
    try {
      const [docTypes, documents] = await Promise.all([
        this.apiService.document_GetDocTypes(this.taskId).toPromise(),
        this.apiService.document_GetFilesForTask(this.taskId).toPromise()
      ]);

      this.docTypes = docTypes || [];
      this.documents = documents || [];
      
      console.log('Loaded doc types:', this.docTypes);
      console.log('Loaded documents:', this.documents);
    } catch (error) {
      console.error('Error loading documents:', error);
    }
  }

  getDocumentsForType(docTypeId: string): any[] {
    return this.documents.filter(doc => doc.docTypeId === docTypeId);
  }

  onDragOver(event: DragEvent, docTypeId: string): void {
    event.preventDefault();
    event.stopPropagation();
    this.dragOverDocType = docTypeId;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.dragOverDocType = null;
  }

  onDrop(event: DragEvent, docTypeId: string): void {
    event.preventDefault();
    event.stopPropagation();
    this.dragOverDocType = null;

    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      this.uploadFiles(files, docTypeId);
    }
  }

  onFileInputChange(event: Event, docTypeId: string): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.uploadFiles(input.files, docTypeId);
      input.value = ''; // Reset input
    }
  }

  uploadFiles(files: FileList, docTypeId: string): void {
    const fileParameters: any[] = [];

    for (let i = 0; i < files.length; i++) {
      const file = files[i];
      fileParameters.push({
        data: file,
        fileName: file.name
      });
    }

    this.apiService.document_UploadFiles(this.taskId, fileParameters, docTypeId).subscribe({
      next: (response) => {
        console.log('Files uploaded successfully:', response);
        this.loadDocuments(); // Reload documents after upload
      },
      error: (err) => {
        console.error('Error uploading files:', err);
        alert('Error uploading files. Please try again.');
      }
    });
  }

  downloadDocument(doc: any): void {
    this.apiService.document_DownloadFile(this.taskId, doc.id).subscribe({
      next: (response) => {
        const blob = response.data;
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = doc.fileName || 'download';
        link.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        console.error('Error downloading file:', err);
        alert('Error downloading file. Please try again.');
      }
    });
  }

  deleteDocument(document: any): void {
    if (confirm(`Are you sure you want to delete "${document.fileName}"?`)) {
      this.apiService.document_DeleteFile(this.taskId, document.id).subscribe({
        next: (response) => {
          console.log('File deleted successfully:', response);
          this.loadDocuments(); // Reload documents after deletion
        },
        error: (err) => {
          console.error('Error deleting file:', err);
          alert('Error deleting file. Please try again.');
        }
      });
    }
  }

  triggerFileInput(docTypeId: string): void {
    const input = document.getElementById(`file-input-${docTypeId}`) as HTMLInputElement;
    if (input) {
      input.click();
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

        const errorMessages: string[] = [];

        Object.entries(this.dynamicForm.controls).forEach(([name, control]) => {
          console.log(name, control.value, control.errors);
          if (control.invalid && control.errors) {
            const field = this.mergedFields
              .flatMap(section => section.fields)
              .find((f: any) => f.fieldId === name);

            const fieldName = field?.label || name.replace(/_/g, ' ').replace(/\b\w/g, l => l.toUpperCase());

            if (field?.validationErrorMessage) {
              errorMessages.push(field.validationErrorMessage);
            } else {
              const errors = Object.keys(control.errors).map(errorKey => {
                switch(errorKey) {
                  case 'required':
                    return `${fieldName} is required`;
                  case 'email':
                    return `${fieldName} must be a valid email`;
                  case 'pattern':
                    return `${fieldName} has an invalid format`;
                  case 'minlength':
                    return `${fieldName} must be at least ${control.errors![errorKey].requiredLength} characters`;
                  case 'maxlength':
                    return `${fieldName} must not exceed ${control.errors![errorKey].requiredLength} characters`;
                  case 'min':
                    return `${fieldName} must be at least ${control.errors![errorKey].min}`;
                  case 'max':
                    return `${fieldName} must not exceed ${control.errors![errorKey].max}`;
                  default:
                    return `${fieldName} is invalid`;
                }
              });

              errorMessages.push(...errors);
            }
          }
        });

        if (errorMessages.length > 0) {
          alert('Please fix the following errors:\n\n' + errorMessages.join('\n'));
        }
        this.dynamicForm.markAllAsTouched();
        return;
    }

    const formFieldDtos: FormFieldDto[] = [];

    this.mergedFields.forEach(section => {
        section.fields.forEach((f: any) => {
            const dto = new FormFieldDto();
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
                        this.router.navigate(['/dashboard']);
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
