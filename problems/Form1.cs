namespace problems
{
    public partial class Form1 : Form
    {
        static int[] ANSWERS = { 3, 1, 3, 4, 1, 1 };
        static bool alreadySubmitted = false;
        int score = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private bool checkName()
        {
            if (string.IsNullOrWhiteSpace(tbxName.Text))
            {
                MessageBox.Show("Please enter your name.");
                return false;
            }
            return true;
        }

        private bool checkID()
        {
            if (string.IsNullOrWhiteSpace(tbxNumber.Text))
            {
                MessageBox.Show("Please enter your ID.");
                return false;
            }

            int studentNumber;
            if (!int.TryParse(tbxNumber.Text, out studentNumber) || studentNumber < 0)
            {
                MessageBox.Show("Please enter a valid ID.");
                return false;
            }

            return true;
        }
        private void calculateScore()
        {
            var panels = this.Controls.OfType<Panel>().ToList();
            panels.Reverse();
            for (int panelIndex = 0; panelIndex < panels.Count; panelIndex++)
            {
                var panel = panels[panelIndex];
                var radioButtons = panel.Controls.OfType<RadioButton>().ToList();
                radioButtons.Reverse();
                for (int buttonIndex = 0; buttonIndex < radioButtons.Count; buttonIndex++)
                {
                    if (radioButtons[buttonIndex].Checked)
                    {
                        if (buttonIndex == ANSWERS[panelIndex] - 1)
                        {
                            score += 10;
                        }
                        break;
                    }
                }
            }
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if(alreadySubmitted)
            {
                MessageBox.Show($"You have already submitted your answers, and score is {score}.");
                return;
            }
            if(!checkName() || !checkID())
            {
                return;
            }
            
            alreadySubmitted = true;
            calculateScore();
            MessageBox.Show($"Your score is {score} out of 60.");
        }

    }
}
