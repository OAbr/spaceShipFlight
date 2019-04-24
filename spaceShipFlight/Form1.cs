using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace spaceShipFlight
{    
    public partial class Form1 : Form
    {
        private bool isReadyFlag = false;
        private bool maxFuelMassFlag = false;
        private bool maxFuelCoefficientFlag = false;
        private bool stopFlightFlag = false;
        private const uint LightSpeed = 299792458;// m/s
        private const ulong LightYear = 9460730470000000;// m
        private const ulong LightHour = 1079252848800;// m
        private double eps = 0.000000001;// g
        private double fuelSpeed;
        private double fuelConsumption;
        private double shipMass;
        private double distance;
        private double maxFuelMass;
        private double maxFuelCoefficient;
        private double accelFuelMass;
        private double accelTime;
        private double accelWay;
        private double decelFuelMass;
        private double decelTime;
        private double decelWay;
        private double maxSpeed;
        private double fullFlightTime;
        private double engineTractionForce;
        private delegate double calcTime(double mass);        
        private ConfigHolder currentConfig = new ConfigHolder();
        private LanguageHolder currenuLanguage;
        private LanguageHolder[] languageList;

        private class ConfigHolder
        {
            public int languageIndex = 0;
            public string currentDataFile = "Data//def.dat";

            private void createConfigIni()
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("config.ini");
                file.Write("Data//def.dat");
                file.Close();
            }

            public void loadConfig()
            {
                if (!System.IO.File.Exists("config.ini")) createConfigIni();
                System.IO.StreamReader file = new System.IO.StreamReader("config.ini");
                currentDataFile = file.ReadLine();
                int.TryParse(file.ReadLine(), out languageIndex); 
                file.Close();
            }

            public void saveConfig()
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("config.ini");
                file.WriteLine(currentDataFile);
                file.WriteLine(languageIndex);
                file.Close();
            }

           /* public double fuelSpeedVal;
            public double fuelConsumptionVal;
            public double shipMassVal;
            public double distanceVal;
            public double maxFuelMassVal;
            public double maxFuelCoefficientVal;
            public bool[] checkIndexes;
            public int[] inputParametersIndexes;
            public int[] outputParametersIndexes;*/
        }

        private class LanguageHolder
        {
            public string sMenuData;
            public string sMenuSave;
            public string sMenuSaveTo;
            public string sMenuLoad;
            public string sMenuLanguage;
            public string sMenuLanguageName;
            public string sMenuExit;
            public string sTab1;
            public string sBox1Name;
            public string sCheck1Name;
            public string sCheck2Name;
            public string sCheck3Name;
            public string sCheck4Name;
            public string sParam1_1Name;
            public string sParam1_2Name;
            public string sParam1_3Name;
            public string sParam1_4Name;
            public string sParam1_5Name;
            public string sParam1_6Name;
            public string sBox2Name;
            public string sParam2_01Name;
            public string sParam2_02Name;
            public string sParam2_03Name;
            public string sParam2_04Name;
            public string sParam2_05Name;
            public string sParam2_06Name;
            public string sParam2_07Name;
            public string sParam2_08Name;
            public string sParam2_09Name;
            public string sParam2_10Name;
            public string sTab2;
            public string sChart1Legend;
            public string sChart2Legend;
            public string sChart3Legend;
            public string sChart4Legend;
            public string sDummyMessage;

        }

        private void initLanguage(int index)
        {
            currentConfig.languageIndex = index;
            currenuLanguage = languageList[index];
            dataToolStripMenuItem.Text = currenuLanguage.sMenuData;
            saveToolStripMenuItem.Text = currenuLanguage.sMenuSave;
            saveToToolStripMenuItem.Text = currenuLanguage.sMenuSaveTo;
            loadToolStripMenuItem.Text = currenuLanguage.sMenuLoad;
            languageToolStripMenuItem.Text = currenuLanguage.sMenuLanguage;
            exitToolStripMenuItem.Text = currenuLanguage.sMenuExit;
            parametersGroupBox.Text = currenuLanguage.sBox1Name;
            fuelSpeedGroupBox.Text = currenuLanguage.sParam1_1Name;
            fuelConsumptionGroupBox.Text = currenuLanguage.sParam1_2Name;
            shipMassGroupBox.Text = currenuLanguage.sParam1_3Name;
            distanceGroupBox.Text = currenuLanguage.sParam1_4Name;
            maxFuelMassGroupBox.Text = currenuLanguage.sParam1_5Name;
            maxFuelCoefficientGroupBox.Text = currenuLanguage.sParam1_6Name;
            calcParametersGroupBox.Text = currenuLanguage.sBox2Name;
            engineTractionForceGroupBox.Text = currenuLanguage.sParam2_01Name;
            maxSpeedGroupBox.Text = currenuLanguage.sParam2_02Name;
            accelFuelMassGroupBox.Text = currenuLanguage.sParam2_03Name;
            accelTimeGroupBox.Text = currenuLanguage.sParam2_04Name;
            accelWayGroupBox.Text = currenuLanguage.sParam2_05Name;
            fullFlightTimeGroupBox.Text = currenuLanguage.sParam2_06Name;
            fullFuelMassGroupBox.Text = currenuLanguage.sParam2_07Name;
            decelFuelMassGroupBox.Text = currenuLanguage.sParam2_08Name;
            decelTimeGroupBox.Text = currenuLanguage.sParam2_09Name;
            decelWayGroupBox.Text = currenuLanguage.sParam2_10Name;
            tabPage1.Text = currenuLanguage.sTab1;
            tabPage2.Text = currenuLanguage.sTab2;
            pCheckedListBox.Items.Clear();
            pCheckedListBox.Items.Insert(0, currenuLanguage.sCheck1Name);
            pCheckedListBox.Items.Insert(1, currenuLanguage.sCheck2Name);
            pCheckedListBox.Items.Insert(2, currenuLanguage.sCheck3Name);
            pCheckedListBox.Items.Insert(3, currenuLanguage.sCheck4Name);
            chart1.Series[0].Name = currenuLanguage.sChart1Legend;
            chart2.Series[0].Name = currenuLanguage.sChart2Legend;
            chart3.Series[0].Name = currenuLanguage.sChart3Legend;
            chart4.Series[0].Name = currenuLanguage.sChart4Legend;
        }

        private void readLanguage(LanguageHolder lang, System.IO.StreamReader file)
        {
            lang.sMenuData         = file.ReadLine();
            lang.sMenuSave         = file.ReadLine();
            lang.sMenuSaveTo       = file.ReadLine();
            lang.sMenuLoad         = file.ReadLine();
            lang.sMenuLanguage     = file.ReadLine();
            lang.sMenuLanguageName = file.ReadLine();
            lang.sMenuExit         = file.ReadLine();
            lang.sTab1             = file.ReadLine();
            lang.sBox1Name         = file.ReadLine();
            lang.sCheck1Name       = file.ReadLine();
            lang.sCheck2Name       = file.ReadLine();
            lang.sCheck3Name       = file.ReadLine();
            lang.sCheck4Name       = file.ReadLine();
            lang.sParam1_1Name     = file.ReadLine();
            lang.sParam1_2Name     = file.ReadLine();
            lang.sParam1_3Name     = file.ReadLine();
            lang.sParam1_4Name     = file.ReadLine();
            lang.sParam1_5Name     = file.ReadLine();
            lang.sParam1_6Name     = file.ReadLine();
            lang.sBox2Name         = file.ReadLine();
            lang.sParam2_01Name    = file.ReadLine();
            lang.sParam2_02Name    = file.ReadLine();
            lang.sParam2_03Name    = file.ReadLine();
            lang.sParam2_04Name    = file.ReadLine();
            lang.sParam2_05Name    = file.ReadLine();
            lang.sParam2_06Name    = file.ReadLine();
            lang.sParam2_07Name    = file.ReadLine();
            lang.sParam2_08Name    = file.ReadLine();
            lang.sParam2_09Name    = file.ReadLine();
            lang.sParam2_10Name    = file.ReadLine();
            lang.sTab2             = file.ReadLine();
            lang.sChart1Legend     = file.ReadLine();
            lang.sChart2Legend     = file.ReadLine();
            lang.sChart3Legend     = file.ReadLine();
            lang.sChart4Legend     = file.ReadLine();
            lang.sDummyMessage     = file.ReadLine();
            file.Close();
        }

        private void loadLanguages()
        {
            
            languageList = new LanguageHolder[3];
            languageList[0] = new LanguageHolder();
            readLanguage(languageList[0], new System.IO.StreamReader("Languages\\english.dat"));
            languageList[1] = new LanguageHolder();
            readLanguage(languageList[1], new System.IO.StreamReader("Languages\\russian.dat"));
            languageList[2] = new LanguageHolder();
            readLanguage(languageList[2], new System.IO.StreamReader("Languages\\ukrainian.dat"));
            currenuLanguage = languageList[0];
        }
        
        private void SetMaxFuelMass(bool flag)
        {
            if (flag)
            {
                maxFuelMassFlag = true;
                maxFuelMassGroupBox.Visible = true;
            }
            else
            {
                maxFuelMassFlag = false;
                maxFuelMassGroupBox.Visible = false;
            }
            calcFlight();
        }

        private void SetMaxFuelCoefficient(bool flag)
        {
            if (flag)
            {
                maxFuelCoefficientFlag = true;
                maxFuelCoefficientGroupBox.Visible = true;
            }
            else
            {
                maxFuelCoefficientFlag = false;
                maxFuelCoefficientGroupBox.Visible = false;
            }
            calcFlight();
        }

        private void SetStopFlight(bool flag)
        {
            if (flag)
            {
                stopFlightFlag = true;
                fullFuelMassGroupBox.Visible = true;
                decelFuelMassGroupBox.Visible = true;
                decelTimeGroupBox.Visible = true;
                decelWayGroupBox.Visible = true;
            }
            else
            {
                stopFlightFlag = false;
                fullFuelMassGroupBox.Visible = false;
                decelFuelMassGroupBox.Visible = false;
                decelTimeGroupBox.Visible = false;
                decelWayGroupBox.Visible = false;
            }
            calcFlight();
        }

        private void setParams()
        {
            setEngineTractionForce();
            setMaxSpeed();
            setFullFlightTime();
            setAccelFuelMass();
            setAccelTime();
            setAccelWay();
            if (stopFlightFlag)
            {
                setFullFuelMass();
                setDecelFuelMass();
                setDecelTime();
                setDecelWay();
            }
        }

        private double rocketTicket(double x)
        {
            double log1 = 1 / Math.Log(x / shipMass + 1);
            return (1 / fuelConsumption - (distance * log1 * log1) / (fuelSpeed * (x + shipMass)) - log1 / fuelConsumption +
                (log1 * log1 * x) / (fuelConsumption * (x + shipMass)));

            //accelWay = (fuelSpeed / fuelConsumption) * (x - shipMass * Math.Log(x / shipMass + 1));
            //accelTime = x / fuelConsumption;
            //maxSpeed = fuelSpeed * Math.Log(x / shipMass +1);
            //fullFlightTime = accelTime + (distance - accelWay) / maxSpeed;

            //double accelWay = (fuelSpeed / fuelConsumption) * (x - shipMass * Math.Log(x / shipMass + 1));
            //double dAccelWay = (fuelSpeed / fuelConsumption) * (1 - shipMass / (x + shipMass));
            //double maxSpeed = fuelSpeed * Math.Log(x / shipMass + 1);
            //double dMaxSpeed = fuelSpeed / (x + shipMass);
            //double tmp = (1 / fuelConsumption - dMaxSpeed * (distance - accelWay) / (maxSpeed * maxSpeed) - dAccelWay / maxSpeed);
            //return tmp;
        }

        private double shipTicket(double x)
        {
            double Md = Math.Sqrt(shipMass+x) * Math.Sqrt(shipMass) - shipMass;
            double dMd = 0.5*shipMass/(Md+shipMass);
            double maxSpeed = fuelSpeed * Math.Log(1+Md/shipMass);
            double dMaxSpeed = fuelSpeed * dMd / (Md + shipMass);
            double tracksLen = fuelSpeed / fuelConsumption * (x - 2*Md);
            double dTracksLen = fuelSpeed / fuelConsumption * ( 1 - 2*dMd);            
            double tmp = (1 / fuelConsumption - dMaxSpeed * (distance - tracksLen) / (maxSpeed * maxSpeed) - dTracksLen / maxSpeed);
            return tmp;
        }

        private double solver(calcTime calc)
        {
            double x1 = 0.0;
            double x2 = 1.0;
            double x;
            double tmp = (double.MaxValue-shipMass)/2;
            /*if (calc(tmp * 2) < 0)
            {
                MessageBox.Show("Too much fuel is required. Fuel is limited to " + (tmp/500000000)+" megatons");
                return tmp * 2;
            }
            else
            {*/
                while ((calc(x2) < 0) && (x2 < tmp))
                {
                    x1 = x2;
                    x2 *= 2;
                }
                if ((calc(x2) < 0))
                {
                    x1 = x2;
                    x2 = tmp*2;
                }
                while ((x2 - x1) > eps*x1)
                {
                    x = (x1 + x2) / 2;
                    if (calc(x) < 0)
                    {
                        x1 = x;
                    }
                    else
                    {
                        x2 = x;
                    }
                }
                return (x1 + x2) / 2;
//            }
        }

        private void calcFlight()
        {           
            if (isReadyFlag)
            {
                engineTractionForce = fuelConsumption * fuelSpeed;
                double maxMass=0;
                if (maxFuelMassFlag) maxMass = maxFuelMass + shipMass;
                if (maxFuelCoefficientFlag)
                    if ((maxMass == 0) || (shipMass * (maxFuelCoefficient + 1) < maxMass)) maxMass = shipMass * (maxFuelCoefficient + 1);
                if (!stopFlightFlag) accelFuelMass = solver(new calcTime(rocketTicket));
                else accelFuelMass = solver(new calcTime(shipTicket));
                if ((maxMass == 0) || (maxMass > (accelFuelMass + shipMass))) maxMass = accelFuelMass + shipMass;
                else accelFuelMass = maxMass - shipMass;
                if (!stopFlightFlag) 
                {
                    accelWay = (fuelSpeed / fuelConsumption) * (maxMass - shipMass * (Math.Log(maxMass / shipMass) +1));
                    accelTime = accelFuelMass / fuelConsumption;
                    maxSpeed = fuelSpeed * Math.Log(maxMass / shipMass);
                    fullFlightTime = accelTime + (distance - accelWay) / maxSpeed;
                    decelTime = 0;
                    decelFuelMass = 0;
                }
                else
                {
                    decelFuelMass = Math.Sqrt(maxMass) * Math.Sqrt(shipMass) - shipMass;
                    decelWay = (fuelSpeed / fuelConsumption) * (-decelFuelMass + (shipMass + decelFuelMass) * Math.Log(decelFuelMass / shipMass + 1));
                    accelFuelMass = maxMass - shipMass - decelFuelMass;
                    accelWay = (fuelSpeed / fuelConsumption) * (accelFuelMass - (shipMass + decelFuelMass) * Math.Log(maxMass / (shipMass + decelFuelMass)));
                    maxSpeed = fuelSpeed * Math.Log(decelFuelMass / shipMass + 1);
                    decelTime = decelFuelMass / fuelConsumption;
                    accelTime = accelFuelMass / fuelConsumption;
                    fullFlightTime = accelTime + decelTime + (distance - accelWay - decelWay) / maxSpeed;
                }
                setParams();
                buildGraph();
            }
        }

        /*private void buildGraph()
        {
            int h1 = 200, h2 = 200, w1 = 300, w2 = 300, steps = 100, borderSize = 5;
            Graphics e = this.CreateGraphics();
           // e.ClipBounds.Left = 0;
          //  e.ClipBounds.Left = 0;
            SolidBrush brush = new SolidBrush(Color.Green);

            e.FillRectangle(brush, new Rectangle(borderSize, borderSize, w1, h1));
            e.FillRectangle(brush, new Rectangle(3*borderSize+w1, borderSize, w2, h1));
            e.FillRectangle(brush, new Rectangle(borderSize, 3*borderSize+h1, w1, h2));
            e.FillRectangle(brush, new Rectangle(3 * borderSize + w1, 3 * borderSize + h1, w2, h2));

            Point[] mass_time  = new Point[steps];
            Point[] dist_time  = new Point[steps];
            Point[] speed_time = new Point[steps];
            Point[] accel_time = new Point[steps];
            
            for (int i = 0; i < steps; i++)
            {
                mass_time[i]  = new Point(3 * i, 2 * i);
                dist_time[i]  = new Point(3 * i, 4 * i);
                speed_time[i] = new Point(3 * i, 6 * i);
                accel_time[i] = new Point(3 * i, 1 * i);
            }

            Pen pen = new Pen(Color.Blue);

            e.DrawCurve(pen, mass_time);
            e.DrawCurve(pen, dist_time);
            e.DrawCurve(pen, speed_time);
            e.DrawCurve(pen, accel_time);

        }*/

        private void buildGraph()
        {
            int h = chart1.Height, steps = 1000;
            double currentMass, currentDist, currentSpeed, currentAccel, currentTime;           
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart4.Series[0].Points.Clear();
            currentMass = shipMass + accelFuelMass + decelFuelMass;
            currentDist = 0;
            currentSpeed = 0;
            currentAccel = 0;
            for (int i = 1; i < steps; i++)
            {               
                chart1.Series[0].Points.AddXY(i * fullFlightTime / steps, currentMass);
                chart2.Series[0].Points.AddXY(i * fullFlightTime / steps, currentDist);
                chart3.Series[0].Points.AddXY(i * fullFlightTime / steps, currentSpeed);
                chart4.Series[0].Points.AddXY(i * fullFlightTime / steps, currentAccel);
                currentTime = i * fullFlightTime / steps;
                if (currentTime < accelTime)
                {
                    currentMass  = shipMass + accelFuelMass + decelFuelMass - currentTime * fuelConsumption;
                    currentAccel = (fuelConsumption * fuelSpeed / currentMass);
                    currentSpeed = -fuelSpeed * Math.Log(currentMass / (shipMass + accelFuelMass + decelFuelMass));
                    currentDist = fuelSpeed * (currentTime + currentMass * Math.Log(currentMass / (shipMass + accelFuelMass + decelFuelMass)) / fuelConsumption);
                }
                else if (currentTime > (fullFlightTime - decelTime))
                {
                    currentMass  = shipMass + decelFuelMass - (currentTime + decelTime - fullFlightTime) * fuelConsumption;
                    currentAccel = -(fuelConsumption * fuelSpeed / currentMass);
                    currentSpeed = maxSpeed + fuelSpeed * Math.Log(currentMass / (shipMass + decelFuelMass));
                    currentDist = accelWay + maxSpeed * (currentTime - accelTime) -
                        fuelSpeed * (currentTime + decelTime - fullFlightTime + currentMass * 
                            Math.Log(currentMass / (shipMass +decelFuelMass)) / fuelConsumption); ;
                }
                else
                {
                    currentMass  = shipMass + decelFuelMass;
                    currentAccel = 0;
                    currentSpeed = maxSpeed;
                    currentDist = accelWay + maxSpeed * (currentTime - accelTime);
                }
            }
        }

        private void init()
        {
            loadLanguages();
            currentConfig.loadConfig();
            initLanguage(currentConfig.languageIndex);
            pCheckedListBox.SetItemChecked(2, true);
            fuelSpeedComboBox.SelectedIndex = 0;
            fuelConsumptionComboBox.SelectedIndex = 0;
            shipMassComboBox.SelectedIndex = 2;
            distanceComboBox.SelectedIndex = 2;
            maxFuelMassComboBox.SelectedIndex = 2;
            setMaxFuelCoefficient();
            engineTractionForceComboBox.SelectedIndex = 0;
            fullFlightTimeComboBox.SelectedIndex = 6;
            maxSpeedComboBox.SelectedIndex = 2;
            fullFuelMassComboBox.SelectedIndex = 2;
            accelFuelMassComboBox.SelectedIndex = 2;
            accelTimeComboBox.SelectedIndex = 3;
            accelWayComboBox.SelectedIndex = 1;
            decelFuelMassComboBox.SelectedIndex = 2;
            decelTimeComboBox.SelectedIndex = 3;
            decelWayComboBox.SelectedIndex = 1;           
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            isReadyFlag = true;
            calcFlight();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            init();
        }

        private void pCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            switch (e.Index)
            {
                case 0: if (e.NewValue == System.Windows.Forms.CheckState.Checked)
                    {
                        SetMaxFuelMass(true);
                    }
                    else
                    {
                        SetMaxFuelMass(false);
                    }
                    break;
                case 1: if (e.NewValue == System.Windows.Forms.CheckState.Checked)
                    {
                        SetMaxFuelCoefficient(true);
                    }
                    else
                    {
                        SetMaxFuelCoefficient(false);
                    }
                    break;
                case 2: if (e.NewValue == System.Windows.Forms.CheckState.Checked)
                    {
                        SetStopFlight(true);
                    }
                    else
                    {
                        SetStopFlight(false);
                    }
                    break;
                case 3: if (e.NewValue == System.Windows.Forms.CheckState.Checked)
                    {
                        e.NewValue = System.Windows.Forms.CheckState.Unchecked;
                    }
                    MessageBox.Show(currenuLanguage.sDummyMessage);
                    break;

            }

        }

        private void setFuelSpeed()
        {
            //0 - m/s, 1 - km/h, 2 - km/s
            double.TryParse(fuelSpeedTextBox.Text, out fuelSpeed);
            if (fuelSpeed == 0)
            {
                fuelSpeedTextBox.Text = fuelSpeedTextBox.Text.Replace('.', ',');
                double.TryParse(fuelSpeedTextBox.Text, out fuelSpeed);
            }
            switch (fuelSpeedComboBox.SelectedIndex)
            {
                case 1: fuelSpeed /= 3.6;
                    break;
                case 2: fuelSpeed *= 1000;
                    break;
            }
            calcFlight();
        }

        private void setFuelConsumption()
        {
            //0 - g/s, 1 - kg/s, 2 - t/s
            double.TryParse(fuelConsumptionTextBox.Text, out fuelConsumption);
            if (fuelConsumption == 0)
            {
                fuelConsumptionTextBox.Text = fuelConsumptionTextBox.Text.Replace('.', ',');
                double.TryParse(fuelConsumptionTextBox.Text, out fuelConsumption);
            }
            switch (fuelConsumptionComboBox.SelectedIndex)
            {
                case 1: fuelConsumption *= 1000;
                    break;
                case 2: fuelConsumption *= 1000000;
                    break;
            } 
            calcFlight();
        }

        private void setShipMass()
        {
            //0 - g, 1 - kg, 2 - t, 3 - kt
            double.TryParse(shipMassTextBox.Text, out shipMass);
            if (shipMass == 0)
            {
                shipMassTextBox.Text = shipMassTextBox.Text.Replace('.', ',');
                double.TryParse(shipMassTextBox.Text, out shipMass);
            }
            switch (shipMassComboBox.SelectedIndex)
            {
                case 1: shipMass *= 1000;
                    break;
                case 2: shipMass *= 1000000;
                    break;
                case 3: shipMass *= 1000000000;
                    break;
            }
            calcFlight();
        }

        private void setDistance()
        {
            //0 - m, 1 - km, 2 - lh, 3 - ly, 4 - pc
            double.TryParse(distanceTextBox.Text, out distance);
            if (distance == 0)
            {
                distanceTextBox.Text = distanceTextBox.Text.Replace('.', ',');
                double.TryParse(distanceTextBox.Text, out distance);
            }
            switch (distanceComboBox.SelectedIndex)
            {
                case 1: distance *= 1000;
                    break;
                case 2: distance *= LightHour;
                    break;
                case 3: distance *= LightYear;
                    break;
                case 4: distance *= 30856775800000000;
                    break;
            }
            calcFlight();
        }

        private void setMaxFuelMass()
        {
            //0 - g, 1 - kg, 2 - t, 3 - kt, 4 - mt
            double.TryParse(maxFuelMassTextBox.Text, out maxFuelMass);
            if (maxFuelMass == 0)
            {
                maxFuelMassTextBox.Text = maxFuelMassTextBox.Text.Replace('.', ',');
                double.TryParse(maxFuelMassTextBox.Text, out maxFuelMass);
            }
            switch (maxFuelMassComboBox.SelectedIndex)
            {
                case 1: maxFuelMass *= 1000;
                    break;
                case 2: maxFuelMass *= 1000000;
                    break;
                case 3: maxFuelMass *= 1000000000;
                    break;
                case 4: maxFuelMass *= 1000000000000;
                    break;
            }
            calcFlight();
        }

        private void setMaxFuelCoefficient()
        {
            double.TryParse(maxFuelCoefficientTextBox.Text, out maxFuelCoefficient);
            if (maxFuelCoefficient == 0)
            {
                maxFuelCoefficientTextBox.Text = maxFuelCoefficientTextBox.Text.Replace('.', ',');
                double.TryParse(maxFuelCoefficientTextBox.Text, out maxFuelCoefficient);
            }
            calcFlight();
        }

        private void setEngineTractionForce()
        {
            //0 - N, 1 - dyn, 2 - kgf, 
            double k = 1;
            switch (engineTractionForceComboBox.SelectedIndex)
            {
                case 0: k = 0.001;
                    break;
                case 1: k = 100;
                    break;
                case 2: k = 0.00010197162;
                    break;
            }
            if (Math.Abs( k * engineTractionForce) < 10E+9)
                engineTractionForceTextBox.Text = (k*engineTractionForce).ToString("F2");
            else engineTractionForceTextBox.Text = (k * engineTractionForce).ToString("e4");
        }

        private void setMaxSpeed()
        {
            //0 - m/s, 1 - km/h, 2 - km/s, 
            double k = 1;
            switch (maxSpeedComboBox.SelectedIndex)
            {
                case 0: k = 1;
                    break;
                case 1: k = 1/3.6;
                    break;
                case 2: k = 0.001;
                    break;
            }
            if (Math.Abs(k * maxSpeed) < 10E+9)
                maxSpeedTextBox.Text = (k * maxSpeed).ToString("F2");
            else maxSpeedTextBox.Text = (k * maxSpeed).ToString("e4");
        }

        private void setFullFlightTime()
        {
            //0 - s, 1 - m, 2 - h, 3 - d, 4 - w, 5 - mon, 6 - y, 7 - cen
            double k = 1;
            switch (fullFlightTimeComboBox.SelectedIndex)
            {
                case 0: k = 1.0 ;
                    break;
                case 1: k = 1.0 / 60;
                    break;
                case 2: k = 1.0 / 3600;
                    break;
                case 3: k = 1.0 / (3600 * 24);
                    break;
                case 4: k = 1.0 / (3600 * 24 * 7);
                    break;
                case 5: k = 1.0 / (3600 * 24 * 30);
                    break;
                case 6: k = 1.0 / (3600 * 24 * 365.25);
                    break;
                case 7: k = 0.01 / (3600 * 24 * 365.25);
                    break;
            }
            if (Math.Abs(k * fullFlightTime) < 10E+9)
                fullFlightTimeTextBox.Text = (k * fullFlightTime).ToString("F2");
            else fullFlightTimeTextBox.Text = (k * fullFlightTime).ToString("e4");
        }

        private void setFullFuelMass()
        {
            //0 - g, 1 - kg, 2 - t, 3 - kt, 4 - mt
            double k = 1;
            switch (fullFuelMassComboBox.SelectedIndex)
            {
                case 0: k = 1;
                    break;
                case 1: k = 0.001;
                    break;
                case 2: k = 0.000001;
                    break;
                case 3: k = 0.000000001;
                    break;
                case 4: k = 0.000000000001;
                    break;
            }
            if (Math.Abs(k * (accelFuelMass + decelFuelMass)) < 10E+9)
                fullFuelMassTextBox.Text = (k * (accelFuelMass + decelFuelMass)).ToString("F2");
            else fullFuelMassTextBox.Text = (k * (accelFuelMass + decelFuelMass)).ToString("e4");
        }

        private void setAccelFuelMass()
        {
            //0 - g, 1 - kg, 2 - t, 3 - kt, 4 - mt
            double k = 1;
            switch (accelFuelMassComboBox.SelectedIndex)
            {
                case 0: k = 1;
                    break;
                case 1: k = 0.001;
                    break;
                case 2: k = 0.000001;
                    break;
                case 3: k = 0.000000001;
                    break;
                case 4: k = 0.000000000001;
                    break;
            }
            if (Math.Abs(k * accelFuelMass) < 10E+9)
                accelFuelMassTextBox.Text = (k * accelFuelMass).ToString("F2");
            else accelFuelMassTextBox.Text = (k * accelFuelMass).ToString("e4");
        }

        private void setAccelTime()
        {
            //0 - s, 1 - m, 2 - h, 3 - d, 4 - w, 5 - mon, 6 - y, 7 - cen
            double k = 1;
            switch (accelTimeComboBox.SelectedIndex)
            {
                case 0: k = 1.0;
                    break;
                case 1: k = 1.0 / 60;
                    break;
                case 2: k = 1.0 / 3600;
                    break;
                case 3: k = 1.0 / (3600 * 24);
                    break;
                case 4: k = 1.0 / (3600 * 24 * 7);
                    break;
                case 5: k = 1.0 / (3600 * 24 * 30);
                    break;
                case 6: k = 1.0 / (3600 * 24 * 365.25);
                    break;
                case 7: k = 0.01 / (3600 * 24 * 365.25);
                    break;
            }
            if (Math.Abs(k * accelTime) < 10E+9)
                accelTimeTextBox.Text = (k * accelTime).ToString("F2");
            else accelTimeTextBox.Text = (k * accelTime).ToString("e4");
        }

        private void setAccelWay()
        {
            //0 - m, 1 - km, 2 - lh, 3 - ly, 4 - pc
            double k = 1;
            switch (accelWayComboBox.SelectedIndex)
            {
                case 0: k = 1.0;
                    break;
                case 1: k = 0.001;
                    break;
                case 2: k = 1.0 / LightHour;
                    break;
                case 3: k = 1.0 / LightYear;
                    break;
                case 4: k = 1.0 / 30856775800000000;
                    break;
            }
            if (Math.Abs(k * accelWay) < 10E+9)
                accelWayTextBox.Text = (k * accelWay).ToString("F2");
            else accelWayTextBox.Text = (k * accelWay).ToString("e4");
        }

        private void setDecelFuelMass()
        {
            //0 - g, 1 - kg, 2 - t, 3 - kt, 4 - mt
            double k = 1;
            switch (decelFuelMassComboBox.SelectedIndex)
            {
                case 0: k = 1;
                    break;
                case 1: k = 0.001;
                    break;
                case 2: k = 0.000001;
                    break;
                case 3: k = 0.000000001;
                    break;
                case 4: k = 0.000000000001;
                    break;
            }
            if (Math.Abs(k * decelFuelMass) < 10E+9)
                decelFuelMassTextBox.Text = (k * decelFuelMass).ToString("F2");
            else decelFuelMassTextBox.Text = (k * decelFuelMass).ToString("e4");
        }

        private void setDecelTime()
        {
            //0 - s, 1 - m, 2 - h, 3 - d, 4 - w, 5 - mon, 6 - y, 7 - cen
            double k = 1;
            switch (decelTimeComboBox.SelectedIndex)
            {
                case 0: k = 1.0;
                    break;
                case 1: k = 1.0 / 60;
                    break;
                case 2: k = 1.0 / 3600;
                    break;
                case 3: k = 1.0 / (3600 * 24);
                    break;
                case 4: k = 1.0 / (3600 * 24 * 7);
                    break;
                case 5: k = 1.0 / (3600 * 24 * 30);
                    break;
                case 6: k = 1.0 / (3600 * 24 * 365.25);
                    break;
                case 7: k = 0.01 / (3600 * 24 * 365.25);
                    break;
            }
            if (Math.Abs(k * decelTime) < 10E+9)
                decelTimeTextBox.Text = (k * decelTime).ToString("F2");
            else decelTimeTextBox.Text = (k * decelTime).ToString("e4");
        }

        private void setDecelWay()
        {
            //0 - m, 1 - km, 2 - lh, 3 - ly, 4 - pc
            double k = 1;
            switch (decelWayComboBox.SelectedIndex)
            {
                case 0: k = 1.0;
                    break;
                case 1: k = 0.001;
                    break;
                case 2: k = 1.0 / LightHour;
                    break;
                case 3: k = 1.0 / LightYear;
                    break;
                case 4: k = 1.0 / 30856775800000000;
                    break;
            }
            decelWayTextBox.Text = (k * decelWay).ToString();
            if (Math.Abs(k * decelWay) < 10E+9)
                decelWayTextBox.Text = (k * decelWay).ToString("F2");
            else decelWayTextBox.Text = (k * decelWay).ToString("e4");
        }      
        
        private void fuelSpeedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFuelSpeed();
        }

        private void fuelConsumptionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFuelConsumption();
            
        }

        private void shipMassComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setShipMass();
        }

        private void distanceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setDistance();
        }

        private void maxFuelMassComboBox_SelectionChanged(object sender, EventArgs e)
        {
            setMaxFuelMass();
        }

        private void fuelSpeedTextBox_TextChanged(object sender, EventArgs e)
        {
            setFuelSpeed();
        }

        private void fuelConsumptionTextBox_TextChanged(object sender, EventArgs e)
        {
            setFuelConsumption();
        }

        private void shipMassTextBox_TextChanged(object sender, EventArgs e)
        {
            setShipMass();
        }

        private void distanceTextBox_TextChanged(object sender, EventArgs e)
        {
            setDistance();
        }

        private void maxFuelMassTextBox_TextChanged(object sender, EventArgs e)
        {
            setMaxFuelMass();
        }

        private void maxFuelCoefficientTextBox_TextChanged(object sender, EventArgs e)
        {
            setMaxFuelCoefficient();
        }

        private void engineTractionForceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setEngineTractionForce();
        }

        private void fullFlightTimeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFullFlightTime();
        }

        private void maxSpeedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setMaxSpeed();
        }

        private void fullFuelMassComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFullFuelMass();
        }

        private void accelFuelMassComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setAccelFuelMass();
        }

        private void accelTimeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setAccelTime();
        }

        private void accelWayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setAccelWay();
        }

        private void decelFuelMassComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setDecelFuelMass();
        }

        private void decelTimeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setDecelTime();
        }

        private void decelWayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setDecelWay();
        }      

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initLanguage(0);
        }

        private void russianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initLanguage(1);
        }

        private void ukrainianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initLanguage(2);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            currentConfig.saveConfig();
        }
    }
}
