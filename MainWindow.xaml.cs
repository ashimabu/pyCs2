using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Runtime.InteropServices;

namespace pyCs2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Python.Runtime.Numpy numpy { get; }
        public MainWindow()
        {
            InitializeComponent();
            

            if(!Directory.Exists(Environment.CurrentDirectory + @"\python.3.6.0"))
            {
                //Directory.CreateDirectory(Environment.CurrentDirectory + @"\python2.2.7.18");
                System.IO.Compression.ZipFile.ExtractToDirectory(Environment.CurrentDirectory + @"\python.3.6.0.zip", 
                    Environment.CurrentDirectory);
            }
            string pyth = Environment.CurrentDirectory + @"\python.3.6.0\tools";
            Environment.SetEnvironmentVariable("PATH", pyth, EnvironmentVariableTarget.Process);
            PythonEngine.PythonHome = pyth;


            Console.WriteLine(PythonEngine.BuildInfo);
            Console.WriteLine(PythonEngine.IsInitialized);
            Console.WriteLine(PythonEngine.ProgramName);
            Console.WriteLine(PythonEngine.PythonHome);
            Console.WriteLine(PythonEngine.PythonPath);

            Console.WriteLine(Runtime.PythonDLL);
            PythonEngine.Initialize();

            numpy = new Python.Runtime.Numpy();

            Console.WriteLine(Environment.GetEnvironmentVariable("TEMP"));
        }

        private void buttonGo_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            PythonEngine.Initialize();
            var ts = PythonEngine.BeginAllowThreads();

            using (Py.GIL())
            {
                try
                {
                    dynamic plt = Py.Import("matplotlib.pyplot");
                    dynamic np = Py.Import("numpy");
                    dynamic pd = Py.Import("pandas");
                    var x = np.array(new List<float> { 1, 2, 3, 4 });
                    var y = np.sin(x);
                    var b = plt.plot(x, y);

                    //dynamic np = Py.Import("numpy");

                    dynamic t = Py.Import("nif");
                    //dynamic byteArray = t.saveFigTest();
                    //var bufOut = np.array(byteArray);//, Py.kw("dtype", numpy.GetNumpyDataType(typeof(byte))));
                    //Console.WriteLine(bufOut);
                    //Console.WriteLine(byteArray);

                    //byte[] ba = (byteArray as byte[]);
                    //Console.WriteLine(ba);
                    //PyObject po = t.InvokeMethod("fileSave", new PyTuple( new PyObject[] { Environment.CurrentDirectory + @"\testImage" }));
                    t.fileSave(Environment.CurrentDirectory + @"\testImage");
                    
                    Console.WriteLine(GC.GetTotalMemory(true));
                    //saved.Dispose();
                    //t.DelItem(t);
                    //t.Dispose();
                    GC.Collect();

                    /*Console.WriteLine(PythonEngine.IsInitialized);
                    GC.Collect();
                    GC.WaitForPendingFinalizers(); // should no block
                    GC.Collect();

                    GC.WaitForPendingFinalizers();

                    PythonEngine.EndAllowThreads(ts);
                    PythonEngine.Shutdown();*/

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }
            //PythonEngine.Shutdown();
            /*PyObject testmethod;
            PyObject module;

            IntPtr pylock = PythonEngine.AcquireLock();
            {
                module = Py.Import("nif");
            }

            PythonEngine.ReleaseLock(pylock);*/

            
        }

        private System.Drawing.Image ToImage(byte[] b)
        {
            using (var ms = new MemoryStream(b))
            {
                return System.Drawing.Image.FromStream(ms);
            }
        }

        private void buttonDisplay_Click(object sender, RoutedEventArgs e)
        {
            //GC.Collect();
            imageBox.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\testImage.png"));
        }
    }
}
