using Core.Enums;
using Core.Models;
using UI.ConsoleManagers;
using Task = System.Threading.Tasks.Task;

namespace UI
{
    public class DutyPermission
    {
        private readonly StateManagerUI _stateManagerUI;
        private readonly TesterUI _testerUI;
        private readonly DeveloperUI _developerUI;
        public DutyPermission(StateManagerUI stateManagerUI, TesterUI testerUI, DeveloperUI developerUI)
        {
            _stateManagerUI = stateManagerUI;
            _testerUI = testerUI;
            _developerUI = developerUI;
        }
        public async Task DutyIdentifier(UserProjectRole row)
        {
            switch (row.Duty)
            {
                case Duty.StateManager:
                    await _stateManagerUI.PerformOperationsAsync(row.UserId, row.ProjectId);
                    break;
                case Duty.Tester:
                    await _testerUI.PerformOperationsAsync(row.UserId, row.ProjectId);
                    break;
                case Duty.Developer:
                    await _developerUI.PerformOperationsAsync(row.UserId, row.ProjectId);
                    break;
                case Duty.Unassigned:
                    break;
                default:
                    throw new Exception("Wrong data input");
            }
        }
    }
}
