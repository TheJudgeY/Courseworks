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
        public async Task DutyIdentifier(User user, Project project)
        {
            switch (user.Duty)
            {
                case Duty.StateManager:
                    await _stateManagerUI.PerformOperationsAsync(user, project);
                    break;
                case Duty.Tester:
                    await _testerUI.PerformOperationsAsync(user, project);
                    break;
                case Duty.Developer:
                    await _developerUI.PerformOperationsAsync(user, project);
                    break;
                case Duty.Unassigned:
                    throw new NotImplementedException();
                default:
                    throw new Exception("Wrong data input");
            }
        }
    }
}
