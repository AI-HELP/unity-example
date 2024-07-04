using System;

namespace AIHelp
{
    public class AIHelpDelegate
    {
        public delegate void AsyncEventListener(String jsonEventData, Action<string> acknowledge);
    }
}