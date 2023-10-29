import _reduxjsToolkit from "@reduxjs/toolkit";
import _react from "react";
import _reactTransitionGroup from "react-transition-group";
import { StoreApi, UseBoundStore } from "zustand";

declare global {
	/**
	 * React Hook 风格函数式组件类型。
	 * @template P - 组件的 Props。
	 * @template E - 继承某个 HTML 原生元素的所有 Attrs。
	 */
	export type FC<P = {}, E extends Element | null = null> = React.FC<Override<
		E extends null ? { children?: ReactNode } : React.HTMLAttributes<E>, P>>;

	/**
	 * React useState 中 setter 函数的类型。
	 * @template T - 参数类型。
	 */
	export type SetState<T> = React.Dispatch<React.SetStateAction<T>>;

	/**
	 * useState 的返回值，即包含表示当前值以及设定该值的函数的元组。可用于实现双向绑定。
	 * @template T - 参数类型。
	 */
	export type StateProperty<T> = [T, SetState<T>];
	// | (T extends unknown[] ? never : T)

	/**
	 * 获取 React 组件的 Props。
	 * @template C - React 函数组件。
	 */
	export type PropsOf<C> = C extends React.FC<infer P> ? P : never;

	/**
	 * 获取 Zustand 状态管理的参数类型。
	 * @template S - Zustand 存储对象。
	 */
	export type ZustandState<S> = S extends UseBoundStore<StoreApi<infer T>> ? T : never;

	export { CSSProperties, ChangeEvent, ChangeEventHandler, DependencyList, EventHandler, MouseEventHandler, ReactElement, ReactNode, RefObject } from "react";
	export { SwitchTransition, TransitionGroup } from "react-transition-group"; // CSSTransition 与原生类重名。
}
