using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpStudy
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class TaskTest
    {
        private ConcurrentQueue<string> _logs = new ConcurrentQueue<string>();

        /// <remarks>
        /// Debug Trace:
        /// start - Main
        /// task called - Main
        /// task running - Async worker
        /// task completed - Continue worker
        /// task completed result - yyyy/MM/dd HH:mm:ss.FFFFFFF
        /// </remarks>
        [TestMethod]
        public void ContinueTest()
        {
            Thread.CurrentThread.Name = "Main";
            _logs.Clear();

            Task<DateTime> SleepAsync(int millisecond)
            {
                return Task.Run(() =>
                {
                    Thread.CurrentThread.Name = "Async worker";
                    _logs.Enqueue($"task running - {Thread.CurrentThread.Name}");
                    Thread.Sleep(millisecond);
                    return DateTime.Now;
                });
            }

            _logs.Enqueue($"start - {Thread.CurrentThread.Name}");
            var task = SleepAsync(1500);
            Task afterTask = task.ContinueWith(t =>
            {
                Thread.CurrentThread.Name = "Continue worker";
                _logs.Enqueue($"task completed - {Thread.CurrentThread.Name}");
                _logs.Enqueue($"task completed result - {task.Result:yyyyMMdd HH:mm:ss.FFFFFFF}");
            });
            _logs.Enqueue($"task called - {Thread.CurrentThread.Name}");

            afterTask.Wait();

            _logs.ToList().ForEach(log => Debug.WriteLine(log));
        }

        [TestMethod]
        public void ExceptionTest()
        {
            _logs.Clear();

            Task ExceptionAsync()
            {
                return Task.Run(() =>
                {
                    Thread.Sleep(1000);
                    throw new Exception("anything error");
                });
            }

            _logs.Enqueue($"start");

            Task task;
            //try
            //{
            //    task = ExceptionAsync();
            //}
            //catch (Exception ex)
            //{
            //    _logs.Enqueue("task exception occured. Can't catch.");
            //    _logs.Enqueue($"Details: {ex}");
            //}
            task = ExceptionAsync();
            var afterTask = task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    _logs.Enqueue("1: task exception occured.");
                    _logs.Enqueue($"1: Details: {t.Exception}");
                }
            });

            afterTask.Wait();

            _logs.ToList().ForEach(log => Debug.WriteLine(log));
        }

        [TestMethod]
        public void ExceptionTest2()
        {
            _logs.Clear();

            Task ExceptionAsync()
            {
                return Task.Run(() =>
                {
                    Thread.Sleep(1000);
                    throw new Exception("anything error");
                });
            }

            _logs.Enqueue($"start");

            var task = ExceptionAsync();

            try
            {
                task.Wait();
            }
            catch (Exception ex)
            {
                _logs.Enqueue("2: task exception occured.");
                _logs.Enqueue($"2: Details: {ex}");
            }

            _logs.ToList().ForEach(log => Debug.WriteLine(log));
        }

        [TestMethod]
        public void ResultTest()
        {
            Task<DateTime> GetTimeAsync()
            {
                return Task.Run(() =>
                {
                    Thread.Sleep(1000);
                    return DateTime.Now;
                });
            }

            var timeTask = GetTimeAsync();

            var after1Task = Task.Run(() =>
            {
                _logs.Enqueue($"time1 - {timeTask.Result:yyyy/MM/dd HH:mm:ss.FFFFFF}");
            });

            var after2Task = Task.Run(() =>
            {
                _logs.Enqueue($"time2 - {timeTask.Result:yyyy/MM/dd HH:mm:ss.FFFFFF}");
            });

            Task.WaitAll(after1Task, after2Task);
            _logs.ToList().ForEach(log => Debug.WriteLine(log));
        }


        [TestMethod]
        public void Exception3Test()
        {
            Task<DateTime> GetTimeAsync()
            {
                return Task.Run(() =>
                {
                    Thread.Sleep(1000);
                    if (true)
                    {
                        throw new Exception("get time error");
                    }
                    return DateTime.Now;
                });
            }

            var timeTask = GetTimeAsync();

            var after1Task = Task.Run(() =>
            {
                try
                {
                    _logs.Enqueue($"time1 - {timeTask.Result:yyyy/MM/dd HH:mm:ss.FFFFFF}");
                }
                catch (Exception ex)
                {
                    _logs.Enqueue($"time1 exception occurd - {ex}");
                }
            });

            var after2Task = Task.Run(() =>
            {
                try
                {
                    _logs.Enqueue($"time2 - {timeTask.Result:yyyy/MM/dd HH:mm:ss.FFFFFF}");
                }
                catch (Exception ex)
                {
                    _logs.Enqueue($"time2 exception occurd - {ex}");
                }
            });

            Task.WaitAll(after1Task, after2Task);
            _logs.ToList().ForEach(log => Debug.WriteLine(log));
        }
    }
}
