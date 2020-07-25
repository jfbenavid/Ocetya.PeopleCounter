import styled from "styled-components";

const border = 9;

export const CounterDiv = styled.div`
	border: green solid ${border}px;
	display: flex;
	align-items: center;
	justify-content: center;
	flex-direction: column;
	padding: 5px;
	min-width: 250px;
`;

export const MaxDiv = styled.div`
	border-top: ${border}px solid green;
	align-items: center;
	display: flex;
	justify-content: center;
	color: red;
	width: 100%;
`;

export const H1 = styled.h1`
	font-size: 5.1rem;
	margin-bottom: 0;
`;

export const H4 = styled.h4`
	margin-bottom: 0;
`;

export const H5 = styled.h5`
	margin-bottom: 0;
`;

export const Span = styled.span`
	margin-right: 10px;
`;

export const H2 = styled.h2`
	margin: 0;
`;
