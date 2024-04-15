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
		inert?: boolean;
	}
}

declare global {
	interface Element {
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/API/Element/scrollIntoViewIfNeeded) */
		scrollIntoViewIfNeeded(centerIfNeeded?: boolean): void;
	}

	interface ImportMeta {
		dirname: string;
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
	 * React Hook style functional component type.
	 * @template P - Props of the component.
	 * @template T - Inherit all Attrs from a native HTML element.
	 */
	export type FC<P = {}, T extends string | null = null> = React.FC<FCP<P, T>>;

	type GetTagFromElement<E> = { [T in keyof HTMLElementTagNameMap]: HTMLElementTagNameMap[T] extends E ? E extends HTMLElementTagNameMap[T] ? T : never : never }[keyof HTMLElementTagNameMap];
	type GetAttributesFromTag<T> = React.ReactDOM[T] extends React.DetailedHTMLFactory<infer A, Any> ? A : never;
	type GetAttributesFromElement<E> = GetAttributesFromTag<GetTagFromElement<E>>;

	/**
	 * React Hook style functional component type.
	 * @deprecated
	 * @template P - Props of the component.
	 * @template E - Inherit all Attrs from a native HTML element.
	 */
	export type FCP_Element<P = {}, E extends Element | null = null> = Override<
		E extends null ? { children?: ReactNode } : GetAttributesFromElement<E>, P>;

	/**
	 * Props type for React Hook style functional components.
	 * @template P - Props of the component.
	 * @template T - Inherit all Attrs from a native HTML element.
	 */
	export type FCP<P = {}, T extends string | null = null> = Override<
		T extends null ? { children?: ReactNode } : GetAttributesFromTag<T>, P>;

	/**
	 * The type of setter function in React useState.
	 * @template T - Parameter type.
	 */
	export type SetState<T> = React.Dispatch<React.SetStateAction<T>> | ((value: T) => unknown);
	// ((value: T) => unknown) | ((value: (prevState: T) => unknown) => unknown);
	export type SetStateNarrow<T> = React.Dispatch<React.SetStateAction<T>>;

	/**
	 * The return value of useState, which contains a tuple representing the current value and the function that sets that value.
	 * Can be used to achieve bidirectional binding.
	 * @template T - Parameter type.
	 */
	export type StateProperty<T> = [get?: T, set?: SetState<T>];
	// | (T extends unknown[] ? never : T)
	/**
	 * The return value of useState, which contains a tuple representing the current value and the function that sets that value.
	 * Can be used to achieve bidirectional binding. But the parameter cannot be empty.
	 * @template T - Parameter type.
	 */
	export type StatePropertyNonNull<T> = [get: T, set: SetState<T>];

	/**
	 * Get Props for the React component.
	 * @template C - React functional component.
	 */
	export type PropsOf<C> = C extends React.FC<infer P> ? P : never;

	/**
	 * Get React Element type for the React component.
	 * @template C - React functional component.
	 */
	export type GetReactElementFromFC<T> = ReactElement<PropsOf<T>, T>;

	/**
	 * Get the parameter types for Zustand store state.
	 * @template S - Zustand store object。
	 */
	export type ZustandState<S> = NonNull<S extends UseBoundStore<StoreApi<infer T>> ? T : never>;

	interface BaseEvent<T = Element> extends React.SyntheticEvent<T>, Event { }
	export type BaseEventHandler<T = Element> = EventHandler<BaseEvent<T>>;

	export type ElementTagNameMap = HTMLElementTagNameMap & SVGElementTagNameMap & MathMLElementTagNameMap & HTMLElementDeprecatedTagNameMap;
	export type TagNameToElement<T extends keyof ElementTagNameMap | Element> = T extends keyof ElementTagNameMap ? ElementTagNameMap[T] : T;
	export type ForwardedRef<T> = T extends keyof ElementTagNameMap ? React.ForwardedRef<ElementTagNameMap[T]> : React.ForwardedRef<T>;

	export type CSSTransitionProps = Partial<ReactTransitionGroupCssTransition.CSSTransitionProps>;
	export type TransitionProps = Partial<ReactTransitionGroupTransition.TransitionProps>;

	export type { Draft } from "immer";
	export type { AnimationItem } from "lottie-web";
	export type { CSSProperties, ChangeEventHandler, DependencyList, DragEventHandler, EventHandler, FormEventHandler, KeyboardEventHandler, MouseEventHandler, MutableRefObject, PointerEventHandler, ReactElement, ReactNode, RefObject, UIEventHandler, WheelEventHandler } from "react";
	export type { SwitchTransition, TransitionGroup } from "react-transition-group"; // CSSTransition 与原生类重名。
	export type { WebTarget } from "styled-components";
}
