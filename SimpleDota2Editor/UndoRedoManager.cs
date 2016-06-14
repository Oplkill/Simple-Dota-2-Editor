using System.Collections.Generic;

namespace SimpleDota2Editor
{
    public class UndoRedoManager
    {
        Stack<ICommand> UndoStack { get; set; }
        Stack<ICommand> RedoStack { get; set; }

        public UndoRedoManager()
        {
            UndoStack = new Stack<ICommand>();
            RedoStack = new Stack<ICommand>();
        }

        public void Undo()
        {
            if (UndoStack.Count > 0)
            {
                //изымаем команду из стека
                var command = UndoStack.Pop();
                //отменяем действие команды
                command.UnExecute();
                //заносим команду в стек Redo
                RedoStack.Push(command);
                //сигнализируем об изменениях
                //StateChanged(this, EventArgs.Empty);
            }
        }

        public void Redo()
        {
            if (RedoStack.Count > 0)
            {
                //изымаем команду из стека
                var command = RedoStack.Pop();
                //выполняем действие команды
                command.Execute();
                //заносим команду в стек Undo
                UndoStack.Push(command);
                //сигнализируем об изменениях
                //StateChanged(this, EventArgs.Empty);
            }
        }

        //выполняем команду
        public void Execute(ICommand command)
        {
            //выполняем команду
            command.Execute();
            //заносим в стек Undo
            UndoStack.Push(command);
            //очищаем стек Redo
            RedoStack.Clear();
            //сигнализируем об изменениях
            //StateChanged(this, EventArgs.Empty);
        }

        public bool CanUndo()
        {
            return UndoStack.Count != 0;
        }

        public bool CanRedo()
        {
            return RedoStack.Count != 0;
        }

        public string GetUndoActionName()
        {
            if (!CanUndo())
                return "";

            return UndoStack.Peek().Name;
        }

        public string GetRedoActionName()
        {
            if (!CanRedo())
                return "";

            return RedoStack.Peek().Name;
        }
    }
}