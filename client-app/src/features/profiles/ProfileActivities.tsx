import { observer } from "mobx-react-lite"
import { UserActivity } from "../../app/models/profile";
import { useStore } from "../../app/stores/store";
import { Card, Grid, GridColumn, Header, Tab, TabPane, TabProps } from "semantic-ui-react";
import { SyntheticEvent, useEffect } from "react";
import UserActivityCard from "./UserActivityCard";

export default observer (function ProfileActivities () {
    const {profileStore} = useStore();
    const {loadUserActivities, 
        loadingActivities, 
        profile, 
        userActivities,
        clearUserActivities} = profileStore;

    const panes = [
        {menuItem: 'Future activities', pane: {key: 'future'}},
        {menuItem: 'Past Events', pane: {key: 'past'}},
        {menuItem: 'Hosting', pane: {key: 'hosting'}}
    ];

    useEffect(() => {
        loadUserActivities(profile!.username);
        return () => clearUserActivities();       
    }, [profile, loadUserActivities])

    const handleTabChange = (_: SyntheticEvent, data: TabProps) => {
        loadUserActivities(profile!.username, panes[data.activeIndex as number].pane.key);
    };

    return (
        <Tab.Pane>
            <Grid>
                <GridColumn width={16}>
                    <Header floated='left' icon='calendar' content={'Activities'} />
                </GridColumn>
                <GridColumn width={16}>
                    <Tab
                        panes={panes}
                        menu={{secondary: true, pointing: true}}
                        onTabChange={(e, data) => handleTabChange(e, data)}                     
                    />
                    <br />
                    <TabPane loading={loadingActivities} style={{border: 0, padding: 0}}>
                        <Card.Group itemsPerRow={4}>
                            {userActivities.map((userActivity: UserActivity) => (
                                <UserActivityCard key={userActivity.id} userActivity={userActivity} />
                            ))}
                        </Card.Group>
                    </TabPane>
                </GridColumn>
            </Grid>
        </Tab.Pane>
    )
})