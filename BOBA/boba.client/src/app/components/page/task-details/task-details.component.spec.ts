import { ComponentFixture, TestBed, fakeAsync, flush } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { TaskDetailsComponent } from './task-details.component';
import { HeaderComponent } from '../../frame/header/header.component';
import {
  ApiService,
  TaskSummaryDto,
  TaskFlowSummaryDto,
  ChoiceSummaryDto,
  TaskDocTypeDto,
  FormDocumentDto,
  TaskFieldDto,
  FormFieldDto,
} from '../../../services/api-service.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

class MockApiService {
  task_GetTaskById = jasmine.createSpy('task_GetTaskById').and.returnValue(
    of(
      Object.assign(new TaskSummaryDto(), {
        id: 'task-1',
        name: 'Test Task',
        currentStateIsFinal: false,
      })
    )
  );

  taskFlow_GetTaskFlow = jasmine
    .createSpy('taskFlow_GetTaskFlow')
    .and.returnValue(
      of(
        Object.assign(new TaskFlowSummaryDto(), {
          id: 'flow-1',
          formFields: [
            {
              layout: 'vertical',
              fields: [
                { fieldId: 'field1', label: 'Field 1', required: true },
                { fieldId: 'field2', label: 'Field 2', required: false },
              ],
            },
          ],
          nextState: [{ choiceId: 'choice-1', nextStateId: 'state-2' }],
        })
      )
    );

  taskFlow_GetChoices = jasmine
    .createSpy('taskFlow_GetChoices')
    .and.returnValue(
      of([
        Object.assign(new ChoiceSummaryDto(), {
          id: 'choice-1',
          name: 'Approve',
        }),
        Object.assign(new ChoiceSummaryDto(), {
          id: 'choice-2',
          name: 'Reject',
        }),
      ])
    );

  taskFlow_GetTaskStateName = jasmine
    .createSpy('taskFlow_GetTaskStateName')
    .and.returnValue(of('Next State'));

  form_GetTaskFieldsByName = jasmine
    .createSpy('form_GetTaskFieldsByName')
    .and.returnValue(
      of([
        Object.assign(new TaskFieldDto(), {
          id: 'tf-1',
          name: 'field1',
          label: 'Field 1',
          required: true,
        }),
        Object.assign(new TaskFieldDto(), {
          id: 'tf-2',
          name: 'field2',
          label: 'Field 2',
          required: false,
        }),
      ])
    );

  form_GetSavedFieldsForTask = jasmine
    .createSpy('form_GetSavedFieldsForTask')
    .and.returnValue(
      of([
        Object.assign(new FormFieldDto(), {
          modelId: 'tf-1',
          value: 'saved value',
        }),
      ])
    );

  form_SaveFormFields = jasmine
    .createSpy('form_SaveFormFields')
    .and.returnValue(of({}));

  task_UpdateTask = jasmine
    .createSpy('task_UpdateTask')
    .and.returnValue(of({}));

  document_GetDocTypes = jasmine
    .createSpy('document_GetDocTypes')
    .and.returnValue(
      of([
        Object.assign(new TaskDocTypeDto(), {
          id: 'doc-type-1',
          name: 'Invoice',
        }),
        Object.assign(new TaskDocTypeDto(), {
          id: 'doc-type-2',
          name: 'Receipt',
        }),
      ])
    );

  document_GetFilesForTask = jasmine
    .createSpy('document_GetFilesForTask')
    .and.returnValue(
      of([
        Object.assign(new FormDocumentDto(), {
          id: 'doc-1',
          fileName: 'file1.pdf',
          docTypeId: 'doc-type-1',
        }),
        Object.assign(new FormDocumentDto(), {
          id: 'doc-2',
          fileName: 'file2.pdf',
          docTypeId: 'doc-type-1',
        }),
      ])
    );

  document_UploadFiles = jasmine
    .createSpy('document_UploadFiles')
    .and.returnValue(of({}));

  document_DownloadFile = jasmine
    .createSpy('document_DownloadFile')
    .and.returnValue(
      of({ data: new Blob(['test'], { type: 'application/pdf' }) })
    );

  document_DeleteFile = jasmine
    .createSpy('document_DeleteFile')
    .and.returnValue(of({}));
}

class MockRouter {
  navigate = jasmine.createSpy('navigate');
  navigateByUrl = jasmine
    .createSpy('navigateByUrl')
    .and.returnValue(Promise.resolve(true));
}

class MockActivatedRoute {
  paramMap = of({
    get: (key: string) => (key === 'id' ? 'task-1' : null),
  });
}

describe('TaskDetailsComponent', () => {
  let component: TaskDetailsComponent;
  let fixture: ComponentFixture<TaskDetailsComponent>;
  let mockApiService: MockApiService;
  let mockRouter: MockRouter;
  let mockActivatedRoute: MockActivatedRoute;

  beforeEach(async () => {
    mockApiService = new MockApiService();
    mockRouter = new MockRouter();
    mockActivatedRoute = new MockActivatedRoute();

    await TestBed.configureTestingModule({
      declarations: [TaskDetailsComponent, HeaderComponent],
      imports: [HttpClientTestingModule, FormsModule, ReactiveFormsModule],
      providers: [
        { provide: ApiService, useValue: mockApiService },
        { provide: Router, useValue: mockRouter },
        { provide: ActivatedRoute, useValue: mockActivatedRoute },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(TaskDetailsComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('loadTask', () => {
    it('should load task successfully', async () => {
      component.taskId = 'task-1';
      await component.loadTask();
      expect(mockApiService.task_GetTaskById).toHaveBeenCalledWith('task-1');
      expect(component.task).toBeDefined();
      expect(component.task.id).toBe('task-1');
    });

    it('should handle error when loading task fails', async () => {
      component.taskId = 'task-1';
      mockApiService.task_GetTaskById.and.returnValue(
        throwError(() => new Error('API Error'))
      );

      try {
        await component.loadTask();
        fail('Expected promise to reject');
      } catch (err) {
        expect(err).toBeTruthy();
      }
    });
  });

  describe('loadTaskFlow', () => {
    it('should load taskflow successfully', async () => {
      component.taskId = 'task-1';
      await component.loadTaskFlow();
      expect(mockApiService.taskFlow_GetTaskFlow).toHaveBeenCalledWith(
        'task-1'
      );
      expect(component.taskflow).toBeDefined();
      expect(component.taskflow.id).toBe('flow-1');
    });

    it('should handle error when loading taskflow fails', async () => {
      component.taskId = 'task-1';
      mockApiService.taskFlow_GetTaskFlow.and.returnValue(
        throwError(() => new Error('API Error'))
      );

      try {
        await component.loadTaskFlow();
        fail('Expected promise to reject');
      } catch (err) {
        expect(err).toBeTruthy();
      }
    });
  });

  describe('buildDynamicForm', () => {
    beforeEach(async () => {
      component.taskId = 'task-1';
      await component.loadTaskFlow();
    });

    it('should build dynamic form with merged fields', async () => {
      await component.buildDynamicForm();

      expect(mockApiService.form_GetTaskFieldsByName).toHaveBeenCalled();
      expect(mockApiService.form_GetSavedFieldsForTask).toHaveBeenCalled();
      expect(component.mergedFields.length).toBe(1);
      expect(component.mergedFields[0].fields.length).toBe(2);
      expect(component.dynamicForm).toBeDefined();
    });

    it('should populate form controls with saved values', async () => {
      await component.buildDynamicForm();

      const field1Control = component.dynamicForm.get('field1');
      expect(field1Control?.value).toBe('saved value');
    });

    it('should handle empty formFields gracefully', async () => {
      component.taskflow = Object.assign(new TaskFlowSummaryDto(), {
        formFields: [],
      });

      await component.buildDynamicForm();
      expect(component.mergedFields.length).toBe(0);
    });

  });

  describe('loadChoices', () => {
    beforeEach(async () => {
      component.taskId = 'task-1';
      await component.loadTaskFlow();
    });

    it('should load choices successfully', async () => {
      await component.loadChoices();

      expect(component.choiceIds).toEqual(['choice-1']);
      expect(mockApiService.taskFlow_GetChoices).toHaveBeenCalledWith([
        'choice-1',
      ]);
      expect(component.choices.length).toBe(2);
    });

    it('should handle error when loading choices fails', async () => {
      mockApiService.taskFlow_GetChoices.and.returnValue(
        throwError(() => new Error('API Error'))
      );

      try {
        await component.loadChoices();
        fail('Expected promise to reject');
      } catch (err) {
        expect(err).toBeTruthy();
      }
    });
  });

  describe('onChoiceSelected', () => {
    beforeEach(async () => {
      component.taskId = 'task-1';
      await component.loadTaskFlow();
    });

    it('should set selected choice and load next state name', () => {
      const choice = Object.assign(new ChoiceSummaryDto(), {
        id: 'choice-1',
        name: 'Approve',
      });

      component.taskflow = {
        nextState: [
          {
            choiceId: 'choice-1',
            nextStateId: 'state-2',
          },
        ],
      } as any;

      component.selectedChoiceId = 'choice-1';

      component.onChoiceSelected(choice);

      expect(component.selectedChoice).toBe(choice);
      // The API should be called with the nextStateId, not the choiceId
      expect(mockApiService.taskFlow_GetTaskStateName).toHaveBeenCalledWith(
        'state-2'
      );
    });

    it('should not load state name if selectedChoiceId is null', () => {
      const choice = Object.assign(new ChoiceSummaryDto(), {
        id: 'choice-1',
        name: 'Approve',
      });
      component.selectedChoiceId = null;

      component.onChoiceSelected(choice);

      expect(component.selectedChoice).toBe(choice);
      expect(mockApiService.taskFlow_GetTaskStateName).not.toHaveBeenCalled();
    });
  });

  describe('loadDocuments', () => {
    beforeEach(() => {
      component.taskId = 'task-1';
    });

    it('should load doc types and documents', async () => {
      await component.loadDocuments();

      expect(mockApiService.document_GetDocTypes).toHaveBeenCalledWith(
        'task-1'
      );
      expect(mockApiService.document_GetFilesForTask).toHaveBeenCalledWith(
        'task-1'
      );
      expect(component.docTypes.length).toBe(2);
      expect(component.documents.length).toBe(2);
    });

    it('should handle errors gracefully', async () => {
      mockApiService.document_GetDocTypes.and.returnValue(
        throwError(() => new Error('API Error'))
      );

      await component.loadDocuments();

      expect(component.docTypes).toEqual([]);
      expect(component.documents).toEqual([]);
    });
  });

  describe('getDocumentsForType', () => {
    beforeEach(() => {
      component.documents = [
        Object.assign(new FormDocumentDto(), {
          id: 'doc-1',
          docTypeId: 'type-1',
        }),
        Object.assign(new FormDocumentDto(), {
          id: 'doc-2',
          docTypeId: 'type-1',
        }),
        Object.assign(new FormDocumentDto(), {
          id: 'doc-3',
          docTypeId: 'type-2',
        }),
      ];
    });

    it('should return documents for specified type', () => {
      const docs = component.getDocumentsForType('type-1');
      expect(docs.length).toBe(2);
    });

    it('should return empty array for non-existent type', () => {
      const docs = component.getDocumentsForType('non-existent');
      expect(docs.length).toBe(0);
    });
  });

  describe('uploadFiles', () => {
    it('should upload files and reload documents', () => {
      component.taskId = 'task-1';
      const file = new File(['content'], 'test.pdf');
      const fileList = {
        length: 1,
        item: (index: number) => (index === 0 ? file : null),
        0: file,
      } as FileList;

      spyOn(component, 'loadDocuments');

      component.uploadFiles(fileList, 'doc-type-1');

      expect(mockApiService.document_UploadFiles).toHaveBeenCalled();
    });
  });

  describe('downloadDocument', () => {
    it('should download document', () => {
      component.taskId = 'task-1';
      const doc = { id: 'doc-1', fileName: 'test.pdf' };

      spyOn(window.URL, 'createObjectURL').and.returnValue('blob:url');
      spyOn(window.URL, 'revokeObjectURL');

      component.downloadDocument(doc);

      expect(mockApiService.document_DownloadFile).toHaveBeenCalledWith(
        'task-1',
        'doc-1'
      );
    });
  });

  describe('deleteDocument', () => {
    it('should delete document after confirmation', () => {
      component.taskId = 'task-1';
      const doc = { id: 'doc-1', fileName: 'test.pdf' };
      spyOn(window, 'confirm').and.returnValue(true);
      spyOn(component, 'loadDocuments');

      component.deleteDocument(doc);

      expect(mockApiService.document_DeleteFile).toHaveBeenCalledWith(
        'task-1',
        'doc-1'
      );
    });

    it('should not delete document if not confirmed', () => {
      const doc = { id: 'doc-1', fileName: 'test.pdf' };
      spyOn(window, 'confirm').and.returnValue(false);

      component.deleteDocument(doc);

      expect(mockApiService.document_DeleteFile).not.toHaveBeenCalled();
    });
  });

  describe('confirmation dialog', () => {
    it('should open confirmation dialog', () => {
      component.openConfirmationDialog();
      expect(component.showConfirmDialog).toBe(true);
    });

    it('should cancel confirmation', () => {
      component.showConfirmDialog = true;
      component.cancelConfirmation();
      expect(component.showConfirmDialog).toBe(false);
    });
  });

  describe('submitTask', () => {
    beforeEach(async () => {
      component.taskId = 'task-1';
      component.selectedChoiceId = 'choice-1';
      await component.loadTaskFlow();
      await component.buildDynamicForm();
    });

    it('should mark all fields as touched when form is invalid', () => {
      component.dynamicForm.get('field1')?.setValue('');
      spyOn(component.dynamicForm, 'markAllAsTouched');
      spyOn(window, 'alert');

      component.submitTask();

      expect(component.dynamicForm.markAllAsTouched).toHaveBeenCalled();
    });
  });

  describe('isFieldRequired', () => {
    beforeEach(async () => {
      component.taskId = 'task-1';
      await component.loadTaskFlow();
      await component.buildDynamicForm();
    });

    it('should return true for required fields', () => {
      expect(component.isFieldRequired('field1')).toBe(true);
    });

    it('should return false for non-required fields', () => {
      expect(component.isFieldRequired('field2')).toBe(false);
    });

    it('should return false for non-existent fields', () => {
      expect(component.isFieldRequired('non-existent')).toBe(false);
    });
  });

  describe('loadData', () => {
    it('should call all load methods in sequence', async () => {
      component.task = Object.assign(new TaskSummaryDto(), {
        currentStateIsFinal: false,
      });

      spyOn(component, 'loadRoute').and.returnValue(Promise.resolve());
      spyOn(component, 'loadTask').and.returnValue(Promise.resolve());
      spyOn(component, 'loadTaskFlow').and.returnValue(Promise.resolve());
      spyOn(component, 'buildDynamicForm').and.returnValue(Promise.resolve());
      spyOn(component, 'loadDocuments').and.returnValue(Promise.resolve());
      spyOn(component, 'loadChoices').and.returnValue(Promise.resolve());

      await component.loadData();

      expect(component.loadRoute).toHaveBeenCalled();
      expect(component.loadTask).toHaveBeenCalled();
      expect(component.loadTaskFlow).toHaveBeenCalled();
      expect(component.buildDynamicForm).toHaveBeenCalled();
      expect(component.loadDocuments).toHaveBeenCalled();
      expect(component.loadChoices).toHaveBeenCalled();
    });

    it('should not load choices when task is in final state', async () => {
      component.task = Object.assign(new TaskSummaryDto(), {
        currentStateIsFinal: true,
      });

      spyOn(component, 'loadRoute').and.returnValue(Promise.resolve());
      spyOn(component, 'loadTask').and.returnValue(Promise.resolve());
      spyOn(component, 'loadTaskFlow').and.returnValue(Promise.resolve());
      spyOn(component, 'buildDynamicForm').and.returnValue(Promise.resolve());
      spyOn(component, 'loadDocuments').and.returnValue(Promise.resolve());
      spyOn(component, 'loadChoices').and.returnValue(Promise.resolve());

      await component.loadData();

      expect(component.loadChoices).not.toHaveBeenCalled();
    });
  });
});
