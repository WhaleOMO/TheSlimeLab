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

public class Style : BaseComponent {
  internal new static Style CreateProxy(IntPtr cPtr, bool cMemoryOwn) {
    return new Style(cPtr, cMemoryOwn);
  }

  internal Style(IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn) {
  }

  internal static HandleRef getCPtr(Style obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  public Style() {
  }

  protected override IntPtr CreateCPtr(Type type, out bool registerExtend) {
    registerExtend = false;
    return NoesisGUI_PINVOKE.new_Style();
  }

  public void Seal() {
    NoesisGUI_PINVOKE.Style_Seal(swigCPtr);
  }

  public Type TargetType {
    set {
      NoesisGUI_PINVOKE.Style_TargetType_set(swigCPtr, new HandleRef(value, (value != null ? Noesis.Extend.GetNativeType(value) : IntPtr.Zero)));
    }
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Style_TargetType_get(swigCPtr);
      if (cPtr != IntPtr.Zero) {
        Noesis.Extend.NativeTypeInfo info = Noesis.Extend.GetNativeTypeInfo(cPtr);
        return info.Type;
      }
      return null;
    }
  }

  public Style BasedOn {
    set {
      NoesisGUI_PINVOKE.Style_BasedOn_set(swigCPtr, Style.getCPtr(value));
    } 
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Style_BasedOn_get(swigCPtr);
      return (Style)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public ResourceDictionary Resources {
    set {
      NoesisGUI_PINVOKE.Style_Resources_set(swigCPtr, ResourceDictionary.getCPtr(value));
    } 
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Style_Resources_get(swigCPtr);
      return (ResourceDictionary)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public SetterBaseCollection Setters {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Style_Setters_get(swigCPtr);
      return (SetterBaseCollection)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public TriggerCollection Triggers {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.Style_Triggers_get(swigCPtr);
      return (TriggerCollection)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public bool CanSeal {
    get {
      bool ret = NoesisGUI_PINVOKE.Style_CanSeal_get(swigCPtr);
      return ret;
    } 
  }

  public bool IsSealed {
    get {
      bool ret = NoesisGUI_PINVOKE.Style_IsSealed_get(swigCPtr);
      return ret;
    } 
  }

  new internal static IntPtr GetStaticType() {
    IntPtr ret = NoesisGUI_PINVOKE.Style_GetStaticType();
    return ret;
  }

}

}

