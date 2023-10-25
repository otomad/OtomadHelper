import { CSSTransition as _CSSTransition } from "react-transition-group";
import { CSSTransitionProps } from "react-transition-group/CSSTransition";

// 使用 CssTransition 以和内置对象 CSSTransition 命名让位。同时解决新版 nodeRef 的问题。
const CssTransition = (props: Partial<CSSTransitionProps>) => {
	const nodeRef = useRef<HTMLElement | undefined>(null);

	return (
		<_CSSTransition {...props} nodeRef={nodeRef} addEndListener={(done: () => void) => nodeRef.current?.addEventListener("transitionend", done, false)}>
			<>
				{
					// @ts-ignore
					React.Children.map(props.children, child => {
						// @ts-ignore
						return React.cloneElement(child, {
							ref: nodeRef,
						});
					})
				}
			</>
		</_CSSTransition>
	);
};

export default CssTransition;
