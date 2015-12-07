﻿using AspectInjector.BuildTask.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace AspectInjector.BuildTask.Models
{
    internal class AsyncPointCut : PointCut
    {
        #region Fields

        private readonly FieldReference _methodArgsRef;
        private readonly FieldReference _originalTypeRef;

        #endregion Fields

        #region Constructors

        public AsyncPointCut(FieldReference originalTypeRef, FieldReference methodArgsRef, ILProcessor processor, Instruction instruction)
            : base(processor, instruction)
        {
            _originalTypeRef = (FieldReference)CreateMemberReference(originalTypeRef);
            _methodArgsRef = (FieldReference)CreateMemberReference(methodArgsRef);
        }

        #endregion Constructors

        #region Methods

        public override PointCut CreatePointCut(Instruction instruction)
        {
            return new AsyncPointCut(_originalTypeRef, _methodArgsRef, Processor, instruction);
        }

        public override void LoadSelfOntoStack()
        {
            base.LoadSelfOntoStack();
            LoadFieldOntoStack(_originalTypeRef);
        }

        //todo:: optimize for full args load as object[]
        public override void LoadParameterOntoStack(ParameterDefinition parameter, TypeReference expectedType = null)
        {
            base.LoadSelfOntoStack();
            LoadFieldOntoStack(_methodArgsRef);

            Processor.InsertBefore(InjectionPoint, CreateInstruction(OpCodes.Ldc_I4, parameter.Index));
            Processor.InsertBefore(InjectionPoint, CreateInstruction(OpCodes.Ldelem_Ref));

            BoxUnboxIfNeeded(parameter.ParameterType, expectedType);
        }

        #endregion Methods
    }
}