//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.10
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


using System;
using System.Runtime.InteropServices;

namespace Noesis
{

public class CheckBox : ToggleButton {
  internal new static CheckBox CreateProxy(IntPtr cPtr, bool cMemoryOwn) {
    return new CheckBox(cPtr, cMemoryOwn);
  }

  internal CheckBox(IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn) {
  }

  internal static HandleRef getCPtr(CheckBox obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  new internal static IntPtr GetStaticType() {
    IntPtr ret = NoesisGUI_PINVOKE.CheckBox_GetStaticType();
    return ret;
  }

  public CheckBox() {
  }

  protected override IntPtr CreateCPtr(Type type, out bool registerExtend) {
    if (type == typeof(CheckBox)) {
      registerExtend = false;
      return NoesisGUI_PINVOKE.new_CheckBox();
    }
    else {
      return base.CreateExtendCPtr(type, out registerExtend);
    }
  }

  internal new static IntPtr Extend(string typeName) {
    return NoesisGUI_PINVOKE.Extend_CheckBox(Marshal.StringToHGlobalAnsi(typeName));
  }
}

}

