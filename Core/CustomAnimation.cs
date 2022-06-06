using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace testWpf.Core
{
    internal class CustomAnimation
    {
        private int _duration;
        private object _animationObject;
        private static bool _isStarted;
        private double _progress;

        public CustomAnimation(object obj)
        {
            this._animationObject = obj;
        }

        public int duraion
        {
            get { return _duration; }
            set { _duration = value; }
        }

        public object animationObject
        {
            get { return _animationObject; }
            set { _animationObject = value; }
        }
        public static bool isStarted
        {
            get { return _isStarted; }
            set { _isStarted = value; }
        }

        public double progress
        {
            get { return _progress; }
            set { _progress = value; }
        }

        public async void ScrollAnimation(Func<double, double> animFormula,int dur, double startOffset, double scrollOffset)
        {
            long start = UnixTimeNow();
            duraion = dur;
            double progress = 0;
            if (!isStarted)
            {
                while (progress >= 0)
                {
                    progress = (await Task.Run(() => Animate(animFormula, duraion, start)));
                    if (progress != -1)
                    {
                        (animationObject as ScrollViewer).ScrollToHorizontalOffset((progress * scrollOffset) + startOffset);
                        isStarted = true;
                    }
                    else
                    {
                        isStarted = false;
                    }

                }

            }

        }

        public async Task SvgAnimation(Func<double, double> animFormula, int dur, string[] needData, Color min, Color max, bool reverse = false)
        {
            long start = UnixTimeNow();
            duraion = dur;
            double progress = 0;
            if (!isStarted)
            {
                while (progress >= 0)
                {
                    progress = await Task.Run(() => Animate(animFormula, duraion, start));
                    if (progress != -1)
                    {
                        Path obj = animationObject as Path;
                        byte a;
                        byte r;
                        byte g;
                        byte b;

                        if (!reverse)
                        {
                            obj.Data = Geometry.Parse(needData[Math.Abs((int)(progress * needData.Length))]);
                            a = (byte)(((max.A - min.A) * progress) + min.A);
                            r = (byte)(((max.R - min.R) * progress) + min.R);
                            g = (byte)(((max.G - min.G) * progress) + min.G);
                            b = (byte)(((max.B - min.B) * progress) + min.B);
                            obj.Fill = new SolidColorBrush(Color.FromArgb(a, r, g, b));
                        }
                        if (reverse)
                        {
                            obj.Data = Geometry.Parse(needData[Math.Abs((int)((needData.Length) - (progress * needData.Length)))]);
                            a = (byte)(((min.A - max.A) * progress) + max.A);
                            r = (byte)(((min.R - max.R) * progress) + max.R);
                            g = (byte)(((min.G - max.G) * progress) + max.G);
                            b = (byte)(((min.B - max.B) * progress) + max.B);
                            obj.Fill = new SolidColorBrush(Color.FromArgb(a, r, g, b));
                        }
                        isStarted = true;
                    }
                    else
                    {
                        isStarted = false;
                    }
                }
            }
        }
        public async Task SvgAnimation(Func<double, double> animFormula, int dur, string[] needData, bool reverse = false)
        {
            long start = UnixTimeNow();
            duraion = dur;
            double progress = 0;
            if (!isStarted)
            {
                while (progress >= 0)
                {
                    progress = await Task.Run(() => Animate(animFormula, duraion, start));
                    if (progress != -1)
                    {
                        Path obj = animationObject as Path;
                        if (!reverse)
                        {
                            obj.Data = Geometry.Parse(needData[Math.Abs((int)(progress * needData.Length))]);
                        }
                        if (reverse)
                        {
                            obj.Data = Geometry.Parse(needData[Math.Abs((int)((needData.Length) - (progress * needData.Length)))]);
                        }
                        isStarted = true;
                    }
                    else
                    {
                        isStarted = false;
                    }
                }
            }
        }

        public async Task SvgAnimation(Func<double, double> animFormula, int dur,  Color minFill, Color maxFill, Color minStroke, Color maxStroke, bool reverse = false)
        {
            long start = UnixTimeNow();
            duraion = dur;
            double progress = 0;
            if (!isStarted)
            {
                while (progress >= 0)
                {
                    progress = await Task.Run(() => Animate(animFormula, duraion, start));
                    if (progress != -1)
                    {
                        Path obj = animationObject as Path;
                        byte a,r,g,b;
                        if (!reverse)
                        {
                            a = (byte)(((maxFill.A - minFill.A) * progress) + minFill.A);
                            r = (byte)(((maxFill.R - minFill.R) * progress) + minFill.R);
                            g = (byte)(((maxFill.G - minFill.G) * progress) + minFill.G);
                            b = (byte)(((maxFill.B - minFill.B) * progress) + minFill.B);
                            obj.Fill = new SolidColorBrush(Color.FromArgb(a, r, g, b));
                            a = (byte)(((maxStroke.A - minStroke.A) * progress) + minStroke.A);
                            r = (byte)(((maxStroke.R - minStroke.R) * progress) + minStroke.R);
                            g = (byte)(((maxStroke.G - minStroke.G) * progress) + minStroke.G);
                            b = (byte)(((maxStroke.B - minStroke.B) * progress) + minStroke.B);
                            obj.Stroke = new SolidColorBrush(Color.FromArgb(a, r, g, b));
                        }
                        if (reverse)
                        {
                            a = (byte)(((minFill.A - maxFill.A) * progress) + maxFill.A);
                            r = (byte)(((minFill.R - maxFill.R) * progress) + maxFill.R);
                            g = (byte)(((minFill.G - maxFill.G) * progress) + maxFill.G);
                            b = (byte)(((minFill.B - maxFill.B) * progress) + maxFill.B);
                            obj.Fill = new SolidColorBrush(Color.FromArgb(a, r, g, b));
                            a = (byte)(((minStroke.A - maxStroke.A) * progress) + maxStroke.A);
                            r = (byte)(((minStroke.R - maxStroke.R) * progress) + maxStroke.R);
                            g = (byte)(((minStroke.G - maxStroke.G) * progress) + maxStroke.G);
                            b = (byte)(((minStroke.B - maxStroke.B) * progress) + maxStroke.B);
                            obj.Stroke = new SolidColorBrush(Color.FromArgb(a, r, g, b));
                        }
                        isStarted = true;
                    }
                    else
                    {
                        isStarted = false;
                    }
                }
            }
        }


        public long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalMilliseconds;
        }


        public double linear(double tF)
        {
            return tF;
        }

        public double easeOut(double tF)
        {
            return 1 - (1 - tF) * (1 - tF);
        }

        public async Task<double> Animate(Func<double, double> myMethodName, double duration, long startTime)
        {
            await Task.Delay(1);
            long startAnim = UnixTimeNow();
            double timeFraction = (startAnim - startTime) / duration;
            if (timeFraction > 1) timeFraction = 1;
            if (timeFraction < 1)
            {
                //return timeFraction;
                //return 1 - (1 - timeFraction) * (1 - timeFraction);
                return myMethodName(timeFraction);
            }
            else
            {
                return -1;
            }
        }
    }
}
