import type _Immer from "immer";
import type _LottieWeb from "lottie-web";
import type React from "react";
import type _ReactTransitionGroup from "react-transition-group";
import type * as ReactTransitionGroupCssTransition from "react-transition-group/CSSTransition";
import type * as ReactTransitionGroupTransition from "react-transition-group/Transition";
import type _StyledComponents from "styled-components";
import type { StoreApi, UseBoundStore } from "zustand";

declare module "react" {
	interface HTMLAttributes {
		disabled?: boolean;
	}
}

declare module "csstype" {
	interface Properties {
		/**
		 * **Custom properties** (sometimes referred to as **CSS variables** or
		 * **cascading variables**) are entities defined by CSS authors that contain
		 * specific values to be reused throughout a document. They are set using
		 * custom property notation (e.g., `--main-color: black;`) and are accessed
		 * using the `var()` function (e.g., `color: var(--main-color);`).
		 *
		 * @see https://developer.mozilla.org/docs/Web/CSS/Using_CSS_custom_properties
		 */
		[customProperty: `--${string}`]: string | number | undefined;
	}
}

declare global {
	/**
	 * React Hook 风格函数式组件类型。
	 * @template P - 组件的 Props。
	 * @template T - 继承某个 HTML 原生元素的所有 Attrs。
	 */
	export type FC<P = {}, T extends string | null = null> = React.FC<FCP<P, T>>;

	type GetTagFromElement<E> = { [T in keyof HTMLElementTagNameMap]: HTMLElementTagNameMap[T] extends E ? E extends HTMLElementTagNameMap[T] ? T : never : never }[keyof HTMLElementTagNameMap];
	type GetAttributesFromTag<T> = React.ReactDOM[T] extends React.DetailedHTMLFactory<infer A, Any> ? A : never;
	type GetAttributesFromElement<E> = GetAttributesFromTag<GetTagFromElement<E>>;

	/**
	 * React Hook 风格函数式组件的 Props 类型。
	 * @deprecated
	 * @template P - 组件的 Props。
	 * @template E - 继承某个 HTML 原生元素的所有 Attrs。
	 */
	export type FCP_Element<P = {}, E extends Element | null = null> = Override<
		E extends null ? { children?: ReactNode } : GetAttributesFromElement<E>, P>;

	/**
	 * React Hook 风格函数式组件的 Props 类型。
	 * @template P - 组件的 Props。
	 * @template T - 继承某个 HTML 原生元素的所有 Attrs。
	 */
	export type FCP<P = {}, T extends string | null = null> = Override<
		T extends null ? { children?: ReactNode } : GetAttributesFromTag<T>, P>;

	/**
	 * React useState 中 setter 函数的类型。
	 * @template T - 参数类型。
	 */
	export type SetState<T> = React.Dispatch<React.SetStateAction<T>> | ((value: T) => unknown);
	// ((value: T) => unknown) | ((value: (prevState: T) => unknown) => unknown);
	export type SetStateNarrow<T> = React.Dispatch<React.SetStateAction<T>>;

	/**
	 * useState 的返回值，即包含表示当前值以及设定该值的函数的元组。可用于实现双向绑定。
	 * @template T - 参数类型。
	 */
	export type StateProperty<T> = [get?: T, set?: SetState<T>];
	// | (T extends unknown[] ? never : T)
	/**
	 * useState 的返回值，即包含表示当前值以及设定该值的函数的元组。可用于实现双向绑定。但参数不可空。
	 * @template T - 参数类型。
	 */
	export type StatePropertyNonNull<T> = [get: T, set: SetState<T>];

	/**
	 * 获取 React 组件的 Props。
	 * @template C - React 函数组件。
	 */
	export type PropsOf<C> = C extends React.FC<infer P> ? P : never;

	/**
	 * 获取 Zustand 状态管理的参数类型。
	 * @template S - Zustand 存储对象。
	 */
	export type ZustandState<S> = NonNull<S extends UseBoundStore<StoreApi<infer T>> ? T : never>;

	interface BaseEvent<T = Element> extends React.SyntheticEvent<T>, Event { }
	export type BaseEventHandler<T = Element> = EventHandler<BaseEvent<T>>;

	export type ElementTagNameMap = HTMLElementTagNameMap & SVGElementTagNameMap & MathMLElementTagNameMap & HTMLElementDeprecatedTagNameMap;
	export type ForwardedRef<T> = T extends keyof ElementTagNameMap ? React.ForwardedRef<ElementTagNameMap[T]> : React.ForwardedRef<T>;

	export type CSSTransitionProps = Partial<ReactTransitionGroupCssTransition.CSSTransitionProps>;
	export type TransitionProps = Partial<ReactTransitionGroupTransition.TransitionProps>;

	export type { Draft } from "immer";
	export type { AnimationItem } from "lottie-web";
	export type { CSSProperties, ChangeEventHandler, DependencyList, DragEventHandler, EventHandler, FormEventHandler, KeyboardEventHandler, MouseEventHandler, MutableRefObject, PointerEventHandler, ReactElement, ReactNode, RefObject, UIEventHandler } from "react";
	export type { SwitchTransition, TransitionGroup } from "react-transition-group"; // CSSTransition 与原生类重名。
	export type { WebTarget } from "styled-components";
}
