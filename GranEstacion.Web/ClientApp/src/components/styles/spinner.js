import styled, { keyframes } from "styled-components";

const rotate360 = keyframes`
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
`;

export const Spinner = styled.div`
	animation: ${rotate360} 1s linear infinite;
	transform: translateZ(0);
	border-top: 3px solid grey;
	border-right: 3px solid grey;
	border-bottom: 3px solid grey;
	border-left: 5px solid black;
	background: transparent;
	width: 75px;
	height: 75px;
	border-radius: 50%;
`;

export const CenteredDiv = styled.div`
	display: flex;
	justify-content: center;
	align-items: center;
	min-height: 150px;
`;
