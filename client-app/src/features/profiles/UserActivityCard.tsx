import { Link } from "react-router-dom";
import { Card, CardContent, CardDescription, CardHeader, CardMeta, Image } from "semantic-ui-react";
import { UserActivity } from "../../app/models/profile";
import { format } from "date-fns";

interface Props {
    userActivity: UserActivity;
}

export default function UserActivityCard({userActivity}: Props){
    return(
        <Card as={Link} 
            key={userActivity.id}
            to={`/activities/${userActivity.id}`}>
            <Image src={`/assets/categoryImages/${userActivity.category}.jpg`} 
                style={{minHeight: 100, objectFit: 'cover'}}/>
            <CardContent textAlign='center'>
                <CardHeader>{userActivity.title}</CardHeader>
                <CardMeta>
                    <div>{format(new Date(userActivity.date), 'do LLL')}</div>
                    <div>{format(new Date(userActivity.date), 'h:mm a')}</div>
                </CardMeta>
                <CardDescription/>
            </CardContent>
        </Card>
    )
}