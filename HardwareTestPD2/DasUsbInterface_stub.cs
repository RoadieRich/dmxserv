using System;
using System.Linq;
using System.Collections.Generic;
using DasUsbInterface;

namespace DasUsbInterface.Stub
{
	/// <summary>
	/// Stub version of DMXInterface, dumps all commands straight to Console.
	/// </summary>
	public class DMXInterfaceStub : DasUsbInterface.DMXInterface
	{
		public DMXInterfaceStub(int universe = 0) : base(universe)
		{}

		protected ReturnCode DoCommand(UsbCommand command, int param, byte[] data)
		{
			Console.WriteLine("Recieved command " + command.ToString());
			if (data != null)
			{
				var nonZeroChannelCount = data.Where(c => c > 0).Count();

				Console.WriteLine("Received " + nonZeroChannelCount + " non-zero channels as data");
			}
			return ReturnCode.Success;
		}
	}
}
