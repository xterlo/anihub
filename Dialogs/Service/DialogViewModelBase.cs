using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testWpf.Dialogs.Service
{
    public abstract class DialogViewModelBase<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public T DialogResult { get; set; }

        public DialogViewModelBase() 
        {

        }


        public void CloseDialogWithResult(IDialogWindow dialog, T result)
        {
            DialogResult = result;

            if (dialog != null)
                dialog.DialogResult = true;
        }
    }
}
