using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemsOfLinearEquations.Domain;
using SystemsOfLinearEquations.ViewModels;
using System.Threading;
using System.Diagnostics;

namespace SystemsOfLinearEquations.WebUI.Controllers
{
    public class SLEController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Index(int dimension, List<String> coefficientsA, List<String> coefficientsB)
        {
            List<double> a = coefficientsA.Select(c => double.Parse(c.Replace('.', ','))).ToList();
            List<double> b = coefficientsB.Select(c => double.Parse(c.Replace('.', ','))).ToList();

            Stopwatch timer = new Stopwatch();
            timer.Start();
            Thread.Sleep(1);
            timer.Stop();
            long ticksInMsCount = timer.ElapsedTicks;
            timer.Reset();

            timer.Start();
            List<double> solution = SystemOfLinearEquations.SolveSquareSystem(dimension, a, b);
            timer.Stop();
            
            Double taskTime = timer.ElapsedTicks;
            taskTime /= ticksInMsCount;
            SolutionModel model = new SolutionModel
                    {
                        Solution = solution,
                        TaskTime = taskTime
                    };
            return View("Solution", model);
        }

        [HttpPost]
        public ActionResult Sample1(int dimension)
        {
            List<double> allCoefficientsA = new List<double>
            {
                18.4,72.15,26,-17.82,34.20,84.16,23,2,-1,58,542,846.2,23.156,895.1,5.64,1564,
                2.13,89.74,-489.63,256,4861,2168,216,846,21.64,48.946,6.8496,0.216,98.74,56.4,5.845,6.8746,
                84,26,196,5.49,2.19,9.845,126,9.84,5.460,649,8976,246,874,249,8.975,5.4,
                321,5,48,65.4,8,52,1,21,684,2,879,564,21.56,987,5.46,984,

                321,5,489,54,21,8,4684,548,21,8,98.45,84,89,44,684,8,
                654,48,5,10,846,150,8746,84,54,9,548,549,546,9875,489,549,
                645,8.465,8.46,846,842,48,30847,546,46,340,87.645,489,3540,5412,4,86,
                121,54,987,56,8.549,568,487,543,549,584,541,549,654,210,549,645,

                654,8,9,54,4,8,6,54,4,8,6,54,8,6,8,4,
                65,484,87,987,54,8,6,5,40,8,95,48,6,4,594,6,
                68,10,54,98,4540,48,9,5,404,4,0.5,499,10,4,8,42,
                15,8.97,1,07,10,89,8,5,0.4,10,84,5,124,8,96,32,

                98,54,8,65,42,14,85,1,5.856,98,54,1,89,76,53,684,
                8,9.87,54,85,56,17,884,41,64,8,4,56,7.8,6,4,18,
                6,58,9,845,4,9,54,84,68,9,65,48,41,3,89,15,
                65,5,489,6.41,489,741,56,5,0.705,46,84.0,8,654,8,415,09
            };
            List<double> allCoefficientsB = new List<double>
            {
                6.9846, 4.8, 18.94, 1.51, 546, 48, 459, 154, 8, 48, 984, 54, 9, 10, 9, 5
            };

            List<double> coefficientsA = new List<double>(dimension * dimension);
            for (int i = 0; i < dimension; i++)
            {
                coefficientsA.AddRange(allCoefficientsA.Skip(dimension * i).Take(dimension));
            }
            List<double> coefficientsB = allCoefficientsB.Take(dimension).ToList();

            return PartialView("_Sample", new SampleModel
                                               {
                                                   coefficientsA = coefficientsA,
                                                   coefficientsB = coefficientsB
                                               });
        }

        [HttpPost]
        public ActionResult Sample2(int dimension)
        {
            List<double> allCoefficientsA = new List<double>
            {
                18.4,0,26,-17.82,34.20,84.16,23,2,-1,58,0,846.2,0,0,5.64,0,
                0,89.74,0,256,0,0,216,846,21.64,0,6.8496,0.216,0,0,0,6.8746,
                84,0,196,5.49,0,0,0,0,5.460,0,0,246,874,0,8.975,5.4,
                0,5,0,0,8,52,1,21,684,2,879,0,21.56,0,0,984,

                321,5,489,54,21,8,4684,548,21,8,98.45,84,89,44,684,8,
                654,48,0,10,0,150,0,84,54,9,0,549,546,0,0,0,
                0,8.465,0,0,842,0,0,0,46,0,0,0,0,5412,4,0,
                121,0,987,0,0,0,487,543,0,0,541,549,654,210,0,645,

                654,8,0,54,4,8,6,54,4,8,6,54,8,6,8,4,
                0,0,87,0,0,0,6,5,40,0,95,48,6,4,0,6,
                68,10,0,98,4540,48,0,5,0,4,0,499,10,4,8,0,
                15,0,1,07,0,0,8,0,0.4,10,84,5,0,8,96,32,

                98,54,8,65,42,14,85,1,0,0,54,1,89,76,53,0,
                8,0,0,85,0,0,0,0,0,8,4,0,7.8,6,4,18,
                0,0,9,0,4,9,0,84,68,9,65,48,41,3,89,0,
                0,5,0,6.41,489,741,56,5,0,46,84.0,8,654,8,415,09
            };
            List<double> allCoefficientsB = new List<double>
            {
                6.9846, 4.8, 0, 1.51, 0, 48, 0, 154, 8, 48, 0, 54, 9, 0, 9, 5
            };

            List<double> coefficientsA = new List<double>(dimension * dimension);
            for (int i = 0; i < dimension; i++)
            {
                coefficientsA.AddRange(allCoefficientsA.Skip(dimension * i).Take(dimension));
            }
            List<double> coefficientsB = allCoefficientsB.Take(dimension).ToList();

            return PartialView("_Sample", new SampleModel
            {
                coefficientsA = coefficientsA,
                coefficientsB = coefficientsB
            });
        }


        public ViewResult About()
        {
            return View();
        }

    }
}
