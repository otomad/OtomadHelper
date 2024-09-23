/// <summary>
/// Since WebView2 swallows the drag event, we can only manually revoke and re-register the drag event function.
/// <see href="https://github.com/yasoonOfficial/outlook-dndprotocol/blob/master/OutlookDndWinForms/Form1.cs" />
/// <see href="https://github.com/OpenLiveWriter/OpenLiveWriter/blob/master/src/managed/OpenLiveWriter.Interop/Com/IDropTarget.cs" />
/// </summary>

using System.Windows.Forms;

using IComDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;
using IDataObject = System.Windows.Forms.IDataObject;

namespace OtomadHelper.Interop;

[ComImport(), Guid("00000122-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IOleDropTarget {
	[PreserveSig]
	int OleDragEnter(
		[In, MarshalAs(UnmanagedType.Interface)]
		object pDataObj,
		[In, MarshalAs(UnmanagedType.U4)]
		int grfKeyState,
		[In, MarshalAs(UnmanagedType.U8)]
		long pt,
		[In, Out]
		ref int pdwEffect
	);

	[PreserveSig]
	int OleDragOver(
		[In, MarshalAs(UnmanagedType.U4)]
		int grfKeyState,
		[In, MarshalAs(UnmanagedType.U8)]
		long pt,
		[In, Out]
		ref int pdwEffect
	);

	[PreserveSig]
	int OleDragLeave();

	[PreserveSig]
	int OleDrop(
		[In, MarshalAs(UnmanagedType.Interface)]
		object pDataObj,
		[In, MarshalAs(UnmanagedType.U4)]
		int grfKeyState,
		[In, MarshalAs(UnmanagedType.U8)]
		long pt,
		[In, Out]
		ref int pdwEffect
	);
}

[StructLayout(LayoutKind.Sequential)]
public struct PointLong {
	public int x;
	public int y;
}

internal class DropTarget : IOleDropTarget {
	private IDataObject? lastDataObject;
	private DragDropEffects lastEffect;
	private readonly IDropTarget owner;

	public DropTarget(IDropTarget owner) {
		this.owner = owner;
	}

	private DragEventArgs? CreateDragEventArgs(object? pDataObj, int grfKeyState, PointLong pt, int pdwEffect) {
		IDataObject? data;
		if (pDataObj is null)
			data = lastDataObject;
		else if (pDataObj is IDataObject winformDataObj)
			data = winformDataObj;
		else if (pDataObj is IComDataObject)
			data = new DataObject(pDataObj);
		else
			return null;
		DragEventArgs args = new(data, grfKeyState, pt.x, pt.y, (DragDropEffects)pdwEffect, lastEffect);
		lastDataObject = data;
		return args;
	}

	private int GetX(long pt) => (int)(((ulong)pt) & 0xffffffffL);

	private int GetY(long pt) => (int)(((ulong)(pt >> 0x20)) & 0xffffffffL);

	int IOleDropTarget.OleDragEnter(object pDataObj, int grfKeyState, long pt, ref int pdwEffect) {
		PointLong pointl = new() {
			x = GetX(pt),
			y = GetY(pt),
		};
		DragEventArgs? e = CreateDragEventArgs(pDataObj, grfKeyState, pointl, pdwEffect);
		if (e is not null) {
			owner.OnDragEnter(e);
			pdwEffect = (int)e.Effect;
			lastEffect = e.Effect;
		} else
			pdwEffect = 0;
		return 0;
	}

	int IOleDropTarget.OleDragLeave() {
		owner.OnDragLeave(EventArgs.Empty);
		return 0;
	}

	int IOleDropTarget.OleDragOver(int grfKeyState, long pt, ref int pdwEffect) {
		PointLong pointl = new() {
			x = GetX(pt),
			y = GetY(pt),
		};
		DragEventArgs e = CreateDragEventArgs(null, grfKeyState, pointl, pdwEffect)!;
		owner.OnDragOver(e);
		pdwEffect = (int)e.Effect;
		lastEffect = e.Effect;
		return 0;
	}

	int IOleDropTarget.OleDrop(object pDataObj, int grfKeyState, long pt, ref int pdwEffect) {
		PointLong pointl = new() {
			x = GetX(pt),
			y = GetY(pt),
		};
		DragEventArgs? e = CreateDragEventArgs(pDataObj, grfKeyState, pointl, pdwEffect);
		if (e is not null) {
			owner.OnDragDrop(e);
			pdwEffect = (int)e.Effect;
		} else
			pdwEffect = 0;
		lastEffect = DragDropEffects.None;
		lastDataObject = null;
		return 0;
	}
}
