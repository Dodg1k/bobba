namespace ConsoleApp1 {
 class Program {
  delegate double Function (double x);
  static void Table (double x1, double dx, double x2, Function f) {
   double y;
   string OutputFormat = "{0:F4}	{1:F4}";
   Console.WriteLine (OutputFormat, "x", "y");
   for (double x = x1; x <= x2; x += dx) {
    y = f (x);
    Console.WriteLine (OutputFormat,x,y);
   }
  }
  static void Main (string[] args) {
   double sin_ (double x) { return Math.Sin (x); }
   double cos_ (double x) { return Math.Cos (x); }

   Console.Clear ();
   Function function = sin_;
   Table (0, Math.PI / 10, Math.PI, function);
   function = cos_;
   Table (0, Math.PI / 10, Math.PI, function);

   Console.ReadKey();
  }
 }
}
