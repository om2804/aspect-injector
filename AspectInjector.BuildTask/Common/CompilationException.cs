﻿using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Linq;

namespace AspectInjector.BuildTask.Common
{
    [Serializable]
    internal class CompilationException : Exception
    {
        public CompilationException(string message, SequencePoint sp)
            : base(message)
        {
            SequencePoint = sp ?? new SequencePoint(new Document(string.Empty));
        }

        public CompilationException(string message, Instruction inst)
            : this(message, inst == null ? null : inst.SequencePoint)
        {
        }

        public CompilationException(string message, MethodReference mr)
            : this(message, mr == null ? null : mr.Resolve().Body.Instructions.FirstOrDefault(i => i.SequencePoint != null))
        {
        }

        public CompilationException(string message, TypeReference tr)
            : this(message, tr == null ? null : tr.Resolve().Methods.FirstOrDefault(m => m.Body.Instructions.Any(i => i.SequencePoint != null)))
        {
        }

        public SequencePoint SequencePoint { get; private set; }
    }
}