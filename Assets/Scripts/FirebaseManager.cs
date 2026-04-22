using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

public class FirebaseManager : MonoBehaviour
{
    // --- ESTA ES LA PARTE NUEVA PARA EL SINGLETON ---
    public static FirebaseManager instance; 

    private FirebaseFirestore db;
    private string documentPath = "player1"; 
    public int monedasActuales = 0; 

    void Awake()
    {
        // Si no hay instancia, esta es la principal
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Evita que se destruya al cambiar de nivel
        }
        else
        {
            Destroy(gameObject); // Si ya existe uno, destruye el duplicado
        }
    }
    // -----------------------------------------------

    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                Debug.Log("¡Firebase Firestore conectado correctamente!");
                CargarMonedasDesdeNube();
            }
            else
            {
                Debug.LogError("No se pudo conectar a Firebase: " + dependencyStatus);
            }
        });
    }

    public void SumarMoneda()
    {
        monedasActuales++;
        ActualizarMonedasEnNube(monedasActuales);

        // Opcional: Avisar al GameManager local para que la UI se actualice
        if (GameManager.instance != null)
        {
            GameManager.instance.monedasTotales = monedasActuales;
        }
    }

    private void ActualizarMonedasEnNube(int cantidad)
    {
        if (db == null) return;

        DocumentReference docRef = db.Collection("players").Document(documentPath);
        Dictionary<string, object> actualizaciones = new Dictionary<string, object>
        {
            { "monedas", cantidad }
        };

        docRef.UpdateAsync(actualizaciones).ContinueWithOnMainThread(task => {
            if (task.IsCompleted) Debug.Log("Nube actualizada: " + cantidad);
        });
    }

    private void CargarMonedasDesdeNube()
    {
        db.Collection("players").Document(documentPath).GetSnapshotAsync().ContinueWithOnMainThread(task => {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists) {
                monedasActuales = snapshot.GetValue<int>("monedas");
                Debug.Log("Monedas cargadas de la nube: " + monedasActuales);
                
                // Sincronizamos con el GameManager local al cargar
                if (GameManager.instance != null)
                {
                    GameManager.instance.monedasTotales = monedasActuales;
                }
            }
        });
    }

    public void GuardarNivelCompletado(string nombreNivel)
    {
        if (db == null) return;

        DocumentReference docRef = db.Collection("players").Document(documentPath);
        
        Dictionary<string, object> actualizacionNivel = new Dictionary<string, object>
        {
            { nombreNivel, true }
        };

        docRef.UpdateAsync(actualizacionNivel).ContinueWithOnMainThread(task => {
            if (task.IsCompleted) 
            {
                Debug.Log("¡Progreso de " + nombreNivel + " guardado en la nube!");
            }
        });
    }
}