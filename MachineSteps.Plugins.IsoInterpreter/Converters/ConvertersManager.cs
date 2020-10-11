using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineSteps.Plugins.IsoInterpreter.Converters
{
    public class ConvertersManager
    {
        private State _state;

        static private SIstructionConverter _sIstructionConverter;

        static private Dictionary<Tuple<string, int>, BaseIstructionConverter<State>> _setVariableIstructionConverters;

        static private Dictionary<int, BaseIstructionConverter<State>> _gIstructionConverters;

        static private Dictionary<int, BaseIstructionConverter<State>> _mIstructionConverters;

        static ConvertersManager()
        {
            Init();
        }

        public ConvertersManager()
        {
            _state = new State();
        }

        private static void Init()
        {
            _sIstructionConverter = new SIstructionConverter();
            _setVariableIstructionConverters = new Dictionary<Tuple<string, int>, BaseIstructionConverter<State>>();
            _gIstructionConverters = new Dictionary<int, BaseIstructionConverter<State>>();
            _mIstructionConverters = new Dictionary<int, BaseIstructionConverter<State>>();

            var types = AppDomain.CurrentDomain
                                 .GetAssemblies()
                                 .SelectMany((a) => a.GetTypes())
                                 .Where((t) => t.IsClass)
                                 .ToList();

            FillSetVariableConvertersDictionary(types);
            FillGIstructionConvertersDictionary(types);
            FillMIstructionConvertersDictionary(types);
        }

        private static void FillGIstructionConvertersDictionary(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var a = GetAttribute<GIstructionConverterAttribute>(type);

                if (a != null)
                {
                    var o = Activator.CreateInstance(type);
                    _gIstructionConverters.Add(a.Index, o as BaseIstructionConverter<State>);
                }
            }
        }

        private static void FillSetVariableConvertersDictionary(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var a = GetAttribute<SetVariableIstructionConverterAttribute>(type);

                if (a != null)
                {
                    var o = Activator.CreateInstance(type);
                    _setVariableIstructionConverters.Add(new Tuple<string, int>(a.Name, a.Index), o as BaseIstructionConverter<State>);
                }
            }
        }

        private static void FillMIstructionConvertersDictionary(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var a = GetAttribute<MIstructionConverterAttribute>(type);

                if (a != null)
                {
                    var o = Activator.CreateInstance(type);
                    _mIstructionConverters.Add(a.Index, o as BaseIstructionConverter<State>);
                }
            }
        }

        private static T GetAttribute<T>(Type t) where T : IndexedBaseConverterAttribute
        {
            return t.GetCustomAttributes(typeof(T), false)
                     .Select(o => o as T)
                     .FirstOrDefault();
        }

        public List<MachineStep> Convert(BaseIstruction istruction)
        {
            List<MachineStep> steps = null;

            if (istruction is SetVariableIstruction svi)
            {
                steps = Convert(svi);
            }
            else if (istruction is MachIstruction)
            {
                if (istruction is MIstruction mi)
                {
                    steps = Convert(mi);
                }
                else if (istruction is SIstruction si)
                {
                    steps = Convert(si);
                }
                else if (istruction is MultipleMachIstructions mmi)
                {
                    steps = Convert(mmi);
                }
            }
            else if (istruction is GIstruction gi)
            {
                steps = Convert(gi);
            }

            if (steps != null && steps.Count() > 0)
            {
                var str = $"line {istruction.LineNumber}";

                steps.ForEach((s) =>
                {
                    if (string.IsNullOrEmpty(s.Description))
                    {
                        s.Description = str;
                    }
                    else
                    {
                        s.Description += $" ({str})";
                    }
                });
            }

            return steps;
        }

        private List<MachineStep> Convert(SetVariableIstruction istruction)
        {
            if (istruction == null) throw new ArgumentNullException($"The istruction is not type {typeof(SetVariableIstruction).Name}");

            if (_setVariableIstructionConverters.TryGetValue(new Tuple<string, int>(istruction.Name, istruction.Index), out BaseIstructionConverter<State> c))
            {
                return c.Convert(istruction, _state);
            }
            else
            {
                return null;
            }
        }

        private List<MachineStep> Convert(MIstruction istruction)
        {
            if (istruction == null) throw new ArgumentNullException($"The istruction is not type {typeof(MIstruction).Name}");

            if (_mIstructionConverters.TryGetValue(istruction.Istruction, out BaseIstructionConverter<State> c))
            {
                return c.Convert(istruction, _state);
            }
            else
            {
                return null;
            }
        }

        private List<MachineStep> Convert(SIstruction istruction)
        {
            if (istruction == null) throw new ArgumentNullException($"The istruction is not type {typeof(SIstruction).Name}");

            return _sIstructionConverter.Convert(istruction, _state);
        }

        private List<MachineStep> Convert(MultipleMachIstructions istruction)
        {
            if (istruction == null) throw new ArgumentNullException($"The istruction is not type {typeof(MultipleMachIstructions).Name}");

            var result = new List<MachineStep>();

            foreach (var item in istruction.Istructions)
            {
                if (item is MIstruction mi)
                {
                    var ms = Convert(mi);

                    if ((ms != null) && (ms.Count() > 0)) result.AddRange(ms);
                }
                else if (item is SIstruction si)
                {
                    var ms = Convert(si);

                    if ((ms != null) && (ms.Count() > 0)) result.AddRange(ms);
                }
            }

            return (result.Count() > 0) ? result : null;
        }

        private List<MachineStep> Convert(GIstruction istruction)
        {
            if (istruction == null) throw new ArgumentNullException($"The istruction is not type {typeof(GIstruction).Name}");

            if (_gIstructionConverters.TryGetValue(istruction.Istruction, out BaseIstructionConverter<State> c))
            {
                return c.Convert(istruction, _state);
            }
            else
            {
                return null;
            }
        }

    }

}
