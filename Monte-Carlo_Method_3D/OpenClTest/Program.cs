using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloo;
using OpenClCalculation;

namespace OpenClTest
{
    class Program
    {
        private const string Source = @"
__kernel void Test(__global float* mat1, __global float* mat2, int iters, int w, int h)
{
       
    private int i = get_global_id(0);
    private int j = get_global_id(1);

    for(private int it = 0; it < iters; it++) {
        if(i >= 1 && i < w - 1 && j >= 1 && j < h - 1) {
            mat2[i*w + j] =
                mat1[(i+0)*w + j - 1] * 1.0 / 5.0 + 
                mat1[(i+0)*w + j + 1] * 1.0 / 5.0 +
                mat1[(i-1)*w + j - 1] * 1.0 / 5.0 +
                mat1[(i+1)*w + j - 1] * 1.0 / 5.0 +
                mat1[(i-1)*w + j - 1] * 1.0 / 20.0 +
                mat1[(i-1)*w + j + 1] * 1.0 / 20.0 +
                mat1[(i+1)*w + j - 1] * 1.0 / 20.0 +
                mat1[(i+1)*w + j + 1] * 1.0 / 20.0;
        }

        barrier(CLK_LOCAL_MEM_FENCE | CLK_GLOBAL_MEM_FENCE);

        if(i >= 1 && i < w - 1 && j >= 1 && j < h - 1) {
            mat1[i*w + j] =
                mat2[(i+0)*w + j - 1] * 1.0 / 5.0 + 
                mat2[(i+0)*w + j + 1] * 1.0 / 5.0 +
                mat2[(i-1)*w + j - 1] * 1.0 / 5.0 +
                mat2[(i+1)*w + j - 1] * 1.0 / 5.0 +
                mat2[(i-1)*w + j - 1] * 1.0 / 20.0 +
                mat2[(i-1)*w + j + 1] * 1.0 / 20.0 +
                mat2[(i+1)*w + j - 1] * 1.0 / 20.0 +
                mat2[(i+1)*w + j + 1] * 1.0 / 20.0;
        }

        barrier(CLK_LOCAL_MEM_FENCE | CLK_GLOBAL_MEM_FENCE);
    }
}
";

        static void Main(string[] args)
        {
            int w = 11, h = 11, sx = 5, sy = 5, iters = 2;

            var properties = new ComputeContextPropertyList(ComputePlatform.Platforms[0]);
            var context = new ComputeContext(ComputeDeviceTypes.All, properties, null, IntPtr.Zero);
            var quene = new ComputeCommandQueue(context, ComputePlatform.Platforms[0].Devices[0], ComputeCommandQueueFlags.None);

            //Компиляция программы
            var prog = new ComputeProgram(context, Source);
            try
            {
                prog.Build(context.Devices, "", null, IntPtr.Zero);
            }
            catch
            {
                Console.WriteLine(prog.GetBuildLog(context.Devices[0]));
                
            }
            //Создание ядра
            var kernel = prog.CreateKernel("Test");


            var mat = new float[w * h];
            for (int i = 0; i < w * h; i++)
            {
                mat[i] = (i == w * sy + sx) ? 1f : 0f;
            }

            var mat1 = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.UseHostPointer, mat);
            var mat2 = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadWrite, w*h);

            kernel.SetMemoryArgument(0, mat1);
            kernel.SetMemoryArgument(1, mat2);
            kernel.SetValueArgument(2, iters);
            kernel.SetValueArgument(3, w);
            kernel.SetValueArgument(4, h);

            quene.Execute(kernel, null, new long[] { (long)h, (long)w }, null, null);

            quene.ReadFromBuffer(mat1, ref mat, true, null);

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    Console.Write($"{mat[i*w+j]:.00} ");
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
