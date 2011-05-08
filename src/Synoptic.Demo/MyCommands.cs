﻿using System;

namespace Synoptic.Demo
{
    internal class MyCommands
    {
        private readonly IMyService _myService;
        private readonly IMyService2 _myService2;

        public MyCommands(IMyService myService, IMyService2 myService2)
        {
            _myService = myService;
            _myService2 = myService2;
        }

        [CommandAction]
        public void RunMyServices(string message)
        {
            Console.WriteLine("RunMyServices");
            Console.WriteLine("  message=" + message);
            Console.WriteLine("  _myService.Hello=" + _myService.Hello(message));
            Console.WriteLine("  _myService2.Hello2=" + _myService2.Hello2(message));
        }

        [CommandAction]
        public void MyCommand(string param1)
        {
            Console.WriteLine("MyCommand");
            Console.WriteLine("  param1=" + param1);
        }

        [CommandAction]
        public void MyCommand2(string camelParamOne)
        {
            Console.WriteLine("MyCommand2");
            Console.WriteLine("  param1=" + camelParamOne);
        }

        [CommandAction(Name = "ex", Description = "Generates an exception.")]
        public void GenerateException()
        {
            throw new Exception("Exception generated.");
        }

        [CommandAction(Name = "cex", Description = "Generates a CommandException.")]
        public void GenerateCommandException()
        {
            throw new CommandActionException("This is caught internally.");
        }

        [CommandAction(Description = "A test command that demonstrates command parameters.")]
        public void MyThirdCommand([CommandParameter(DefaultValue = "defaultforparam1")] string param1, int param2, [CommandParameter(Description = "this is a test parameter")] bool param3)
        {
            Console.WriteLine("MyThirdCommand");
            Console.WriteLine("  param1=" + param1);
            Console.WriteLine("  param2=" + param2);
            Console.WriteLine("  param3=" + param3);
        }
    }
}