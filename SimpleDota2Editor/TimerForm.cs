using System;

namespace SimpleDota2Editor
{
    public class TimerForm
    {
        public System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer();

        /// <summary>
        /// Добавить функцию
        /// </summary>
        public void AddFunction(EventHandler func)
        {
            Timer.Tick += func;
        }

        /// <summary>
        /// Запуск таймера
        /// </summary>
        /// <param name="interval">Переодичность таймера в мс</param>
        public void Start(int interval)
        {
            Timer.Interval = interval;
            Timer.Start();
        }

        /// <summary>
        /// Остановить таймер
        /// </summary>
        public void Stop()
        {
            Timer.Stop();
        }

        public TimerForm()
        {
            
        }

        /// <summary>
        /// Инициирование с функцией
        /// </summary>
        /// <param name="func"></param>
        public TimerForm(EventHandler func)
        {
            this.AddFunction(func);
        }

        /// <summary>
        /// Инициирование и запуск таймер
        /// </summary>
        public TimerForm(EventHandler func, int interval)
        {
            this.AddFunction(func);
            this.Start(interval);
        }
    }
}