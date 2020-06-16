using ArgumentMarshalerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ArgumentsLib
{
    public class Reflector
    {
        private readonly string marshalerPath;

        private IEnumerable<string> filePath;
        private List<Assembly> assemblies;
        private List<Type> types;

        public Reflector(string marshalerPath)
        {
            if (!Directory.Exists(marshalerPath))
                throw new LibraryArgumentException(ErrorCode.GLOBAL, $"Marshaler Directory: {marshalerPath} not found!");

            this.marshalerPath = marshalerPath;
            this.assemblies = new List<Assembly>();
            this.types = new List<Type>();

            SetFilePaths();
            LoadAssemblies();
            SetTypes();
        }

        private void SetFilePaths()
        {
            filePath = Directory.GetFiles(marshalerPath, "*MarshalerLib.dll");

            if (filePath.Count() == 0)
                throw new LibraryArgumentException(ErrorCode.GLOBAL, $"Marshaler Directory: {marshalerPath} does not contain *MarshalerLib.dll files!");
        }

        private void LoadAssemblies()
        {
            foreach (string path in filePath)
            {
                assemblies.Add(Assembly.LoadFrom(path));
            }
        }

        private void SetTypes()
        {
            foreach (Assembly assembly in assemblies)
            {
                types.Add(assembly.GetType(assembly.DefinedTypes.First().FullName));
            }
        }

        public ArgumentMarshaler GetInstanceBySchema(string schema)
        {
            if (schema == null)
                throw new LibraryArgumentException(ErrorCode.INVALID_SCHEMA, null);

            Type correctType = null;

            foreach (Type type in types)
            {
                object instance = Activator.CreateInstance(type);
                PropertyInfo instanceInfo = type.GetProperty("Schema");
                string value = instanceInfo.GetValue(instance).ToString();

                if(value == null)
                    throw new LibraryArgumentException(ErrorCode.INVALID_SCHEMA, null);

                if (schema == value)
                {
                    correctType = type;
                    break;
                }
            }

            return (ArgumentMarshaler)Activator.CreateInstance(correctType);
        }
    }
}
