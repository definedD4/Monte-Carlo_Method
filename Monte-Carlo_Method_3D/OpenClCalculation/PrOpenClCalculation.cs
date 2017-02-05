using System;
using System.Linq;
using Cloo;

namespace OpenClCalculation
{
    public class PrOpenClCalculation
    {
        private const string ProgramSource = @"
__kernel void Test(__global int* result, __global int* a, __global int* b)          
{
    int glX = get_global_id(0);
       
    result[glX] = a[glX] + b[glX];
}   
";
        private ComputePlatform m_Platform;
        private ComputeDevice m_Device;
        private ComputeContext m_ComputeContext;
        private ComputeCommandQueue m_CommandQueue;
        private ComputeProgram m_Program;
        private ComputeKernel m_Kernel;

        public PrOpenClCalculation()
        {
            m_Platform = ComputePlatform.Platforms.First();
            m_Device = m_Platform.Devices.First();
            m_ComputeContext = new ComputeContext(new [] {m_Device}, new ComputeContextPropertyList(m_Platform), null, IntPtr.Zero);
            m_CommandQueue = new ComputeCommandQueue(m_ComputeContext, m_Device, ComputeCommandQueueFlags.None);
            m_Program = new ComputeProgram(m_ComputeContext, ProgramSource);
            m_Program.Build(new [] {m_Device}, "", null, IntPtr.Zero);
            m_Kernel = m_Program.CreateKernel("Test");

            long count = 100;

            var result = new int[count];
            var a = new int[count];
            for (int i = 0; i < count; i++)
            {
                a[i] = i;
            }

            var resultDev = new ComputeBuffer<int>(m_ComputeContext,
                                                     ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.UseHostPointer,
                                                     result);

            var aDev = new ComputeBuffer<int>(m_ComputeContext,
                                                    ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.UseHostPointer,
                                                    a);


            var bDev = new ComputeBuffer<int>(m_ComputeContext,
                                                    ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.UseHostPointer,
                                                    a);
            //Задаем их для нашего ядра
            m_Kernel.SetMemoryArgument(0, resultDev);
            m_Kernel.SetMemoryArgument(1, aDev);
            m_Kernel.SetMemoryArgument(2, bDev);

            //Вызываем ядро количество потоков равно count
            m_CommandQueue.Execute(m_Kernel, null, new[] { count }, null, null);

            //Читаем результат из переменной
            m_CommandQueue.ReadFromBuffer(resultDev, ref result, true, null);

            //Выводим результат
            foreach (var i in result)
            {
                Console.WriteLine(i);
            }
        }
    }
}