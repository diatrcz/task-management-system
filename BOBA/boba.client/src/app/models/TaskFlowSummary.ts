export interface TaskFlowSummary {
    id: string;
    nextState: {
      choiceId: string;
      nextStateId: string;
    }[];
    editRoleId: string | null;
    readOnlyRole: string[];
  }
  