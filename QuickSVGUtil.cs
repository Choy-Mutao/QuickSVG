using System;
using System.Diagnostics;

namespace QuickSVG
{
    public class PathD : List<double[]>
    {
        private PathD()
        {
        }

        public PathD(int capacity = 0)
            : base(capacity)
        {
        }

        public PathD(IEnumerable<double[]> path)
            : base(path)
        {
            if (path.Any(item => item.Length != 2)) throw new ArgumentException("PathD only contains 2d array");
        }


        public string ToString(int precision = 2)
        {
            string text = "";
            using Enumerator enumerator = GetEnumerator();
            while (enumerator.MoveNext()) text += string.Join(",", enumerator.Current.Select(num => num.ToString("F" + precision)));
            return text;
        }
    }

    public class PathsD : List<PathD>
    {
        private PathsD() { }


        public PathsD(int capacity = 0)
            : base(capacity)
        {
        }

        public PathsD(IEnumerable<PathD> paths)
            : base(paths)
        {
        }

        public string ToString(int precision = 2)
        {
            string text = "";
            using Enumerator enumerator = GetEnumerator();
            while (enumerator.MoveNext())
            {
                PathD current = enumerator.Current;
                text = text + current.ToString(precision) + "\n";
            }

            return text;
        }

    }

    public enum FillRule
    {
        EvenOdd,
        NonZero,
        Positive,
        Negative
    }

    public static class QuickSVGUtil
    {
        public static void AddCaption(QuickSVGWriter svg, string caption, int x, int y)
        {
            svg.AddText(caption, x, y, 14);
        }

        public static void AddSubject(QuickSVGWriter svg, PathD path)
        {
            PathsD paths = [path];
            svg.AddClosedPaths(paths, 0x1800009C, 0xAAB3B3DA, 0.8);
        }

        public static void AddSubject(QuickSVGWriter svg, PathsD paths)
        {
            svg.AddClosedPaths(paths, 0x1800009C, 0xAAB3B3DA, 0.8);
        }
        public static void AddOpenSubject(QuickSVGWriter svg, PathD paths)
        {
            svg.AddOpenPath(paths, 0xAAB3B3DA, 1.2);
        }

        public static void AddOpenSubject(QuickSVGWriter svg, PathsD paths)
        {
            svg.AddOpenPaths(paths, 0xAAB3B3DA, 1.2);
        }

        public static void AddClip(QuickSVGWriter svg, PathD path)
        {
            PathsD paths = [path];
            svg.AddClosedPaths(paths, 0x129C0000, 0xCCFFA07A, 0.8);
        }

        public static void AddClip(QuickSVGWriter svg, PathsD paths)
        {
            svg.AddClosedPaths(paths, 0x129C0000, 0xCCFFA07A, 0.8);
        }
        public static void AddSolution(QuickSVGWriter svg, PathsD paths, bool show_coords = true)
        {
            svg.AddClosedPaths(paths, 0x4080ff9C, 0xFF003300, 1.5, show_coords);
        }

        public static void AddOpenSolution(QuickSVGWriter svg, PathsD paths, bool show_coords = true)
        {
            svg.AddOpenPaths(paths, 0xFF003300, 2.2, show_coords);
        }

        public static void AddSolution(QuickSVGWriter svg, PathD path, bool show_coords = true)
        {
            svg.AddClosedPath(path, 0x4080ff9C, 0xFF003300, 1.5, show_coords);
        }

        public static void AddOpenSolution(QuickSVGWriter svg, PathD path, bool show_coords = true)
        {
            svg.AddOpenPath(path, 0xFF003300, 2.2, show_coords);
        }

        public static void SaveToFile(QuickSVGWriter svg,
          string filename, FillRule fill_rule,
          int max_width = 0, int max_height = 0, int margin = 0)
        {
            if (File.Exists(filename)) File.Delete(filename);
            svg.FillRule = fill_rule;
            svg.SaveToFile(filename, max_width, max_height, margin);
        }

        public static int OpenFileWithDefaultApp(string filename)
        {
            string path = Path.GetFullPath(filename);
            if (!File.Exists(path)) return -1;
            Process p = new() { StartInfo = new ProcessStartInfo(path) { UseShellExecute = true } };
            p.Start();
            p.WaitForExit();
            return p.ExitCode;
        }
    }
}
