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
using System.Collections.Generic;
using System.Text;
using System.CodeDom;
using System.Runtime.InteropServices;

namespace WmcSoft.CodeDom
{

    [Serializable, ComVisible(true), ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class CodeAssertArgumentNotNulllStatement : CodeConditionStatement
    {
        #region Private fields

        CodeVariableReferenceExpression variable;
        CodePrimitiveExpression primitive;

        #endregion

        #region Lifecycle

        public CodeAssertArgumentNotNulllStatement(CodeParameterDeclarationExpression parameter) {
            if (parameter == null) {
                throw new ArgumentNullException("parameter");
            }
            Initialize(parameter.Name);
        }

        public CodeAssertArgumentNotNulllStatement(string argumentName) {
            if (argumentName == null) {
                throw new ArgumentNullException("argumentName");
            }
            Initialize(argumentName);
        }

        private void Initialize(string argumentName) {
            variable = new CodeVariableReferenceExpression(argumentName);
            this.Condition = new CodeNullEqualityExpression(variable);
            primitive = new CodePrimitiveExpression(argumentName);
            this.TrueStatements.Add(
                new CodeThrowNewExceptionStatement<ArgumentNullException>(primitive));
        }

        #endregion

        #region Properties

        public string ArgumentName {
            get {
                return variable.VariableName;
            }
            set {
                variable.VariableName = value;
                primitive.Value = value;
            }
        }

        #endregion

    }

}
