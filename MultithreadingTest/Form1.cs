namespace MultithreadingTest
{
    public partial class Form1 : Form
    {
        private int m_count;
        private int m_reading;
        private int m_lastReading;
        private int m_intervalMs1 = 1;
        private int m_intervalMs2 = 1;
        private EventWaitHandle m_stopEvent = new EventWaitHandle(false, EventResetMode.ManualReset);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_count =
            m_reading =
            m_lastReading = 0;

            // Write thread.
            Task.Run(() =>
            {
                while (m_stopEvent.WaitOne(m_intervalMs1) == false)
                {
                    Increment();
                }
            });

            // Read thread.
            Task.Run(() =>
            {
                while (m_stopEvent.WaitOne(m_intervalMs2) == false)
                {
                    Inspect();
                }
            });
        }

        private void Increment()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => Increment()));
            }
            else
            {
                textBox1.Text = $"{m_count}";

                m_count++;

                if (m_count % 10 == 0)
                {
                    m_reading = m_count;
                }
            }
        }

        private void Inspect()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => Inspect()));
            }
            else
            {
                if (m_reading != m_lastReading)
                {
                    m_lastReading = m_reading;

                    textBox2.AppendText($"{m_lastReading}  ");
                }
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            m_stopEvent.Set();

            textBox2.AppendText("Thread stopped.");
        }
    }
}