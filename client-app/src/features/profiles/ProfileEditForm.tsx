import { Formik } from "formik";
import { Button } from "semantic-ui-react";
import * as Yup from "yup";
import { useStore } from "../../app/stores/store";
import { Form } from "react-router-dom";
import MyTextInput from "../../app/common/form/MyTextInput";
import MyTextArea from "../../app/common/form/MyTextArea";


interface Props {
    setEditMode: (editMode: boolean) => void;
}

export default function ProfileEditForm ({setEditMode} : Props) {
    const {profileStore: {profile, updateProfile}} = useStore();
    
    const validationSchema = Yup.object({
        displayName: Yup.string().required()
    })

    return(
       <Formik
            initialValues={{displayName: profile?.displayName, bio: profile?.bio}} 
            validationSchema={validationSchema}
            onSubmit={values => {
                updateProfile(values).then(() => {
                    setEditMode(false);
                })}}        
        >
        {({handleSubmit, isSubmitting, isValid, dirty}) => (
            <Form className="ui form" onSubmit={handleSubmit}>
                <MyTextInput placeholder="Display Name" name="displayName" />
                <MyTextArea placeholder="Add your bio" name="bio" rows={3} />
                <Button 
                    positive
                    type='submit'
                    loading={isSubmitting}
                    content='Update profile'
                    floated='right'
                    disabled={!isValid || !dirty}
                />
            </Form>
        )}
        </Formik>
    )
}