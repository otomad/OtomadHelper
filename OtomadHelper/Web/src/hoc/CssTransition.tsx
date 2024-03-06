import { CSSTransition as _CSSTransition } from "react-transition-group";

/**
 * 使用 CssTransition 以和内置对象 CSSTransition 命名让位。同时解决新版 React 中要求使用的 nodeRef 的问题。
 */
const CssTransition = forwardRef<HTMLElement, CSSTransitionProps>((props, ref) => {
	const nodeRef = useDomRef<"section">();

	useImperativeHandle(ref, () => nodeRef.current!, []);

	return (
		<_CSSTransition {...props} {...(props.timeout !== undefined ? { timeout: props.timeout } : { nodeRef, addEndListener: endListener(nodeRef) })}>
			{cloneRef(props.children as ReactNode, nodeRef)}
		</_CSSTransition>
	);
}) as FC<CSSTransitionProps>;

export default CssTransition;
