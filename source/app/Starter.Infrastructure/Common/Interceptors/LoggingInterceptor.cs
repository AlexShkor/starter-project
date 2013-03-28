﻿using System;
using DQF.Platform.Dispatching;
using DQF.Platform.Dispatching.Interfaces;
using DQF.Platform.Domain.Interfaces;
using DQF.Platform.Logging;

namespace DQF.Common.Interceptors
{
    /// <summary>
    /// This interceptors simply intercept every invocation of handler 
    /// in order to write logs about commands and events
    /// </summary>
    public class LoggingInterceptor : IMessageHandlerInterceptor
    {
        private readonly LogManager _logManager;

        public LoggingInterceptor(LogManager logManager)
        {
            _logManager = logManager;
        }

        public void Intercept(DispatcherInvocationContext context)
        {
            if (context.Message == null)
                return;

            var command = context.Message as ICommand;

            if (command != null)
            {
                try
                {
                    _logManager.LogCommand(command);
                    context.Invoke();
                    _logManager.LogCommandHandler(command.Metadata.CommandId, context.Handler.GetType().FullName);
                }
                catch (Exception ex)
                {
                    _logManager.LogCommandHandler(command.Metadata.CommandId, context.Handler.GetType().FullName, ex);
                    throw;
                }

                return;
            }

            var evnt = context.Message as IEvent;

            if (evnt != null)
            {
                try
                {
                    _logManager.LogEvent(evnt);
                    context.Invoke();
                    _logManager.LogEventHandler(evnt.Metadata.CommandId, evnt.Metadata.EventId, context.Handler.GetType().FullName);
                }
                catch (Exception ex)
                {
                    _logManager.LogEventHandler(evnt.Metadata.CommandId, evnt.Metadata.EventId, context.Handler.GetType().FullName, ex);
                    throw;
                }

                return;
            }

            // if this is not command or event - execute as usual
            context.Invoke();
        }
    }
}