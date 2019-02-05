#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

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
using System.CodeDom;
using System.Runtime.InteropServices;

namespace WmcSoft.CodeDom
{
    /// <summary>
    /// Represents a code statement guarding a parameter against null value.
    /// </summary>
    /// <remarks>Generates <code>if(arg == null) throw new ArgmentNullException("arg");</code></remarks>
    [Serializable, ComVisible(true), ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class CodeGuardArgumentNotNulllStatement : CodeConditionStatement
    {
        #region Private fields

        private readonly CodeVariableReferenceExpression _variable;
        private readonly CodePrimitiveExpression _primitive;

        #endregion

        #region Lifecycle

        public CodeGuardArgumentNotNulllStatement(CodeParameterDeclarationExpression parameter)
            : this(parameter == null ? null : parameter.Name)
        {
        }

        public CodeGuardArgumentNotNulllStatement(string argumentName)
        {
            _variable = new CodeVariableReferenceExpression(argumentName);
            _primitive = new CodePrimitiveExpression(argumentName);
            Condition = new CodeNullEqualityExpression(_variable);
            TrueStatements.Add(new CodeThrowNewExceptionStatement<ArgumentNullException>(_primitive));
        }

        #endregion

        #region Properties

        public string ArgumentName {
            get {
                return _variable.VariableName;
            }
            set {
                _variable.VariableName = value;
                _primitive.Value = value;
            }
        }

        #endregion
    }
}
