using Org.BouncyCastle.Utilities;
using System.Runtime.InteropServices;
using System.Text;
using TripSplitAPI;

//[DllImport("pdf_gen.dll", CallingConvention = CallingConvention.Cdecl)]
//unsafe static extern void run(byte* input, int inputLenght, byte* output, int outputLenght);
//string input = "test";
//string output = "report.pdf";

//byte[] inputByteArray = Encoding.UTF8.GetBytes(input);
//byte[] outputByteArray = Encoding.UTF8.GetBytes(output);

//int inputLen = inputByteArray.Length;
//int outputLen = outputByteArray.Length;

//unsafe
//{

//    fixed (byte* inputPtr = inputByteArray)
//    {
//        fixed (byte* outputPtr = outputByteArray)
//        {
//            run(inputPtr, inputLen, outputPtr, outputLen);
//        }
//    }
//}

var builder = WebApplication.CreateBuilder(args);

Startup.ConfigureServices(builder);

var app = builder.Build();

Startup.ConfigureApp(app);

app.Run();
