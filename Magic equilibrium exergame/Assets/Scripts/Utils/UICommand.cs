using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utils
{
    public class UICommand
    {
        // Private fields
        private readonly Func<UniTask> _command;
        private readonly Action<Exception> _onException;



        // Initialization
        public UICommand(Func<UniTask> command, Action<Exception> onException = null)
        {
            _command = command;
            _onException = onException;
        }



        // Properties
        public bool IsExecuting { get; private set; }



        // Core
        public void Execute()
        {
            if (IsExecuting)
                return;

            ExecuteAsync().Forget();
        }

        private async UniTaskVoid ExecuteAsync()
        {
            IsExecuting = true;
            try
            {
                await _command.Invoke();
            }
            catch (Exception e)
            {
                _onException?.Invoke(e);
            }
            finally
            {
                IsExecuting = false;
            }
        }
    }
}
