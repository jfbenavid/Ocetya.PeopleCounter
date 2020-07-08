import styled from "styled-components";

export const CounterDiv = styled.div`
	display: flex;
	align-items: center;
	justify-content: center;
	flex-direction: column;
`;

export const MaxDiv = styled.div`
	align-items: center;
	display: flex;
	justify-content: space-between;
`;

export const H1 = styled.h1`
	color: ${({ max, children }) => {
		const div = max / 3;
		if (children < div) return "black";
		else if (children < div * 2) return "yellow";
		else return "red";
	}};
	font-size: 5.1rem;
	margin-bottom: 0;
`;

export const H4 = styled.h4`
	margin-bottom: 0;
`;

export const H5 = styled.h5`
	color: limegreen;
	margin-bottom: 0;
`;

export const Span = styled.span`
	margin-right: 10px;
`;

export const H2 = styled.h2`
	margin: 0;
`;
