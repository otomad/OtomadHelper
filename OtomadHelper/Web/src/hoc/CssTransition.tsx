import { CSSTransition as _CSSTransition } from "react-transition-group";
import type { CSSTransitionProps } from "react-transition-group/CSSTransition";

// 使用 CssTransition 以和内置对象 CSSTransition 命名让位。同时解决新版 nodeRef 的问题。
const CssTransition = forwardRef<HTMLElement, Partial<CSSTransitionProps>>((props, ref) => {
	const nodeRef = useRef<HTMLElement | undefined>(null);

	useImperativeHandle(ref, () => nodeRef.current!, []);

	return (
		<_CSSTransition {...props} nodeRef={nodeRef} addEndListener={endListener(nodeRef)}>
			<>
				{
					React.Children.map(props.children as ReactNode, child => {
						return React.cloneElement(child as ReactElement, {
							ref: nodeRef,
						});
					})
				}
			</>
		</_CSSTransition>
	);
});

export default CssTransition;
