import { CSSTransition as _CSSTransition } from "react-transition-group";
import type { CSSTransitionProps } from "react-transition-group/CSSTransition";

const hasRefInReactNode = (reactNode: unknown): reactNode is { ref: MutableRefObject<HTMLElement> } =>
	!!(reactNode && typeof reactNode === "object" && "ref" in reactNode && reactNode.ref);

/**
 * 使用 CssTransition 以和内置对象 CSSTransition 命名让位。同时解决新版 React 中要求使用的 nodeRef 的问题。
 */
const CssTransition = forwardRef<HTMLElement, Partial<CSSTransitionProps>>((props, ref) => {
	const nodeRef = useRef<HTMLElement | undefined>(null);

	useImperativeHandle(ref, () => nodeRef.current!, []);

	return (
		<_CSSTransition {...props} nodeRef={nodeRef} addEndListener={endListener(nodeRef)}>
			<>
				{
					React.Children.map(props.children as ReactNode, child => {
						if (hasRefInReactNode(child))
							useImperativeHandle(child.ref, () => nodeRef.current!, []);
						return React.cloneElement(child as ReactElement, {
							ref: nodeRef,
						});
					})
				}
			</>
		</_CSSTransition>
	);
}) as FC<Partial<CSSTransitionProps>>;

export default CssTransition;
