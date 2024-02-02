import React from "react";
import { Dimmer, Loader } from "semantic-ui-react";

export interface Props {
    inverted?: boolean;
    content?: string;
}

export default function LoadingConponent({inverted = true, content = '...Loading'}: Props){
    return (
        <Dimmer active={true} inverted={inverted}>
            <Loader content={content} />
        </Dimmer>
    )
}