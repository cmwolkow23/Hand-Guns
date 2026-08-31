using UnityEngine;

public class SaveOnExit : MonoBehaviour
{
    public bool bSkipSave;
    private Vector3 position,rotation;
    private void Awake()
    {
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
    }
    public void Save()
    {
        if(transform.position != position || rotation != transform.rotation.eulerAngles)
        {           
            GameMaster.CurrentSave.SaveObjectState(new ObjectState(gameObject.name + transform.gameObject.scene.name,
                                transform.position,transform.rotation.eulerAngles));
        } 
        else if(GameMaster.CurrentSave.TryGetObjectState(gameObject.name + gameObject.scene.name,out ObjectState oState))
        {
            if(position != oState.position || rotation != oState.rotation)
            {
                GameMaster.CurrentSave.SaveObjectState(new ObjectState(gameObject.name + transform.gameObject.scene.name,
                                transform.position,transform.rotation.eulerAngles));
            }
        }
    }
    private void OnDisable()
    {
        if(bSkipSave)return;
        Save();
    }
}
