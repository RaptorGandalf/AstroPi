using AstroPi.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace AstroPi.Serial
{
    public enum MicroController
    {
        HeadController,
        BodyController
    }

    public static class SerialManager
    {
        private static Dictionary<MicroController, string> _controllers;

        public static async Task Initialize()
        {
            var selector = SerialDevice.GetDeviceSelector();

            var deviceCollection = await DeviceInformation.FindAllAsync(selector);

            foreach (var item in deviceCollection)
            {
                var serialDevice = await SerialDevice.FromIdAsync(item.Id);

                if(serialDevice != null && item.Name.Contains("USB Serial"))
                {
                    serialDevice.IsDataTerminalReadyEnabled = true;
                    serialDevice.IsRequestToSendEnabled = true;

                    await Task.Delay(2000);

                    serialDevice.BaudRate = 9600;
                    serialDevice.DataBits = 8;
                    serialDevice.StopBits = SerialStopBitCount.Two;
                    serialDevice.Parity = SerialParity.None;

                    var dataWriter = new DataWriter(serialDevice.OutputStream);

                    dataWriter.WriteString(SerialConstants.ActivationCode);

                    await dataWriter.StoreAsync();

                    dataWriter.DetachStream();

                    dataWriter.Dispose();

                    await Task.Delay(2000);

                    //dataWriter = new DataWriter(serialDevice.OutputStream);

                    //dataWriter.WriteByte(3);

                    //await dataWriter.StoreAsync();

                    //await Task.Delay(2000);

                    //dataWriter = new DataWriter(serialDevice.OutputStream);

                    //dataWriter.WriteString(SerialCommands.WhoAreYou);

                    //await dataWriter.StoreAsync();

                    //for (int i = 0; i < 3; i++)
                    //{
                    //    var dataReader = new DataReader(serialDevice.InputStream);

                    //    var bytesRecieved = await dataReader.LoadAsync(128);

                    //    var text = dataReader.ReadString(bytesRecieved).Trim();

                    //    Logger.Log("Recieved On Serial: " + text);

                    //    await Task.Delay(500);
                    //}
                }
            }
        }
    }
}
