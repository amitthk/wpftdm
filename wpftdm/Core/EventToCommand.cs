using System;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows;
using System.Reflection;

namespace wpftdm.Core
{
        public class EventToCommand : MarkupExtension
        {
            public string Command { get; set; }
            public string CommandParameter { get; set; }

            public EventToCommand():base()
            {

            }

            public EventToCommand(string command, string commandParameter)
                : base()
            {
                Command=Command;
                CommandParameter=CommandParameter;
            }

            public override object ProvideValue(IServiceProvider serviceProvider)
            {
                var targetProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
                if (targetProvider == null)
                {
                    throw new InvalidOperationException();
                }

                var targetObject = targetProvider.TargetObject as FrameworkElement;
                if (targetObject == null)
                {
                    throw new InvalidOperationException();
                }

                var memberInfo = targetProvider.TargetProperty as MemberInfo;
                if (memberInfo == null)
                {
                    throw new InvalidOperationException();
                }

                if (string.IsNullOrWhiteSpace(Command))
                {
                    Command = memberInfo.Name.Replace("Add", "");
                    if (Command.Contains("Handler"))
                    {
                        Command = Command.Replace("Handler", "Command");
                    }
                    else
                    {
                        Command = Command + "Command";
                    }
                }

                return CreateHandler(memberInfo, Command, targetObject.GetType());
            }

            private Type GetEventHandlerType(MemberInfo memberInfo)
            {
                Type eventHandlerType = null;
                if (memberInfo is EventInfo)
                {
                    var info = memberInfo as EventInfo;
                    var eventInfo = info;
                    eventHandlerType = eventInfo.EventHandlerType;
                }
                else if (memberInfo is MethodInfo)
                {
                    var info = memberInfo as MethodInfo;
                    var methodInfo = info;
                    ParameterInfo[] pars = methodInfo.GetParameters();
                    eventHandlerType = pars[1].ParameterType;
                }

                return eventHandlerType;
            }

            private object CreateHandler(MemberInfo memberInfo, string cmdName, Type targetType)
            {
                Type eventHandlerType = GetEventHandlerType(memberInfo);

                if (eventHandlerType == null) return null;

                var handlerInfo = eventHandlerType.GetMethod("Invoke");
                var method = new DynamicMethod("", handlerInfo.ReturnType,
                    new Type[]
                {
                    handlerInfo.GetParameters()[0].ParameterType,
                    handlerInfo.GetParameters()[1].ParameterType,
                });

                var ilgenrator = method.GetILGenerator();
                ilgenrator.Emit(OpCodes.Ldarg, 0);
                ilgenrator.Emit(OpCodes.Ldarg, 1);
                ilgenrator.Emit(OpCodes.Ldstr, cmdName);
                if (CommandParameter == null)
                {
                    ilgenrator.Emit(OpCodes.Ldnull);
                }
                else
                {
                    ilgenrator.Emit(OpCodes.Ldstr, CommandParameter);
                }
                ilgenrator.Emit(OpCodes.Call, getMethod);
                ilgenrator.Emit(OpCodes.Ret);

                return method.CreateDelegate(eventHandlerType);
            }

            static readonly MethodInfo getMethod = typeof(EventToCommand).GetMethod("AttachHandler", new Type[] { typeof(object), typeof(object), typeof(string), typeof(string) });

            static void Handler(object sender, object args)
            {
                AttachHandler(sender, args, "cmd", null);
            }

            public static void AttachHandler(object sender, object args, string cmdName, string commandParameter)
            {
                var fe = sender as FrameworkElement;
                if (fe != null)
                {
                    ICommand cmd = GetCommand(fe, cmdName);
                    object _commandParam = null;
                    if (!string.IsNullOrWhiteSpace(commandParameter))
                    {
                        _commandParam = GetCommandParameter(sender, commandParameter, sender.GetType());
                    }
                    if ((cmd != null) && cmd.CanExecute(_commandParam))
                    {
                        cmd.Execute(_commandParam);
                    }
                }
            }

            internal static ICommand GetCommand(FrameworkElement target, string cmdName)
            {
                var vm = FindViewModel(target);
                if (vm == null) return null;

                var vmType = vm.GetType();
                var cmdProp = vmType.GetProperty(cmdName);
                if (cmdProp != null)
                {
                    return cmdProp.GetValue(vm) as ICommand;
                }
                else
                {
#if DEBUG
                    throw new Exception("EventToCommand error: '" + cmdName + "' property not found on '" + vmType + "'");
#endif
                    return null;
                }
            }

            internal static BaseViewModel FindViewModel(FrameworkElement target)
            {
                if (target == null) return null;

                var vm = target.DataContext as BaseViewModel;
                if (vm != null) return vm;

                var parent = target.GetParentObject() as FrameworkElement;

                return FindViewModel(parent);
            }

            internal static object GetCommandParameter(object target, string path, Type valueType = null)
            {
                if (target == null) throw new ArgumentNullException("target FrameworkElement is null");
                if (path == null) throw new ArgumentNullException("path to Parameter is null");

                Type currentType = valueType ?? target.GetType();

                foreach (string propertyName in path.Split('.'))
                {
                    PropertyInfo property = currentType.GetProperty(propertyName);
                    if (property == null) throw new NullReferenceException(string.Format("property {0} of object type {1} is null",currentType.ToString(),propertyName));

                    target = property.GetValue(target);
                    currentType = property.PropertyType;
                }
                return target;
            }
        }
}
