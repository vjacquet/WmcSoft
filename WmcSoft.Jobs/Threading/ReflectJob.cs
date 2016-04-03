#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace WmcSoft.Threading
{
    /// <summary>
    /// Represents a job whose purpose is to instanciate and execute another one.
    /// </summary>
    /// <remarks>This class is usefull to avoid dependencies when passing job between applications through message queues.</remarks>
    [Serializable]
    public class ReflectJob : JobBase
    {
        static IDictionary<string, ConstructorInfo> constructors;
        static object syncRoot;

        static ReflectJob() {
            syncRoot = new object();
            constructors = new Dictionary<string, ConstructorInfo>();
        }

        static ConstructorInfo GetConstructor(string typeName, IEnumerable<Type> types) {
            lock (syncRoot) {
                ConstructorInfo ctor;
                if (!constructors.TryGetValue(typeName, out ctor)) {
                    Type type = Type.GetType(typeName, true);
                    Type[] parameters = types.ToArray();
                    ctor = type.GetConstructor(parameters);
                    if (ctor == null) {
                        StringBuilder sb = new StringBuilder(".ctor(");
                        if (parameters.Length > 0) {
                            sb.Append(parameters[0].FullName);
                            for (int i = 1; i < parameters.Length; i++) {
                                sb.Append(", ").Append(parameters[i].FullName);
                            }
                        }
                        sb.Append(")");
                        throw new MissingMethodException(type.FullName, sb.ToString());
                    }
                    constructors.Add(typeName, ctor);
                }
                return ctor;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectJob"/> class.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="parameters">The parameters.</param>
        public ReflectJob(string typeName, params object[] parameters) {
            TypeName = typeName;
            Parameters = parameters;
        }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <value>
        /// The name of the type.
        /// </value>
        public string TypeName { get; private set; }
        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public object[] Parameters { get; private set; }

        /// <summary>
        /// Instanciates the job of the specified type and parameters and executes it.
        /// </summary>
        /// <param name="serviceProvider">An <see cref="System.Object"/> that implements <see cref="System.IServiceProvider"/>.</param>
        protected override void DoExecute(IServiceProvider serviceProvider) {
            var instance = Instance;
            instance.Execute(serviceProvider);
        }

        /// <summary>
        /// Gets the job instance.
        /// </summary>
        public IJob Instance {
            get {
                if (instance == null) {
                    var type = Type.GetType(TypeName, true);
                    var ctor = GetConstructor(TypeName, Parameters.Select(o => o.GetType()));
                    instance = (IJob)ctor.Invoke(Parameters);
                }
                return instance;
            }
        }
        IJob instance;
    }
}
