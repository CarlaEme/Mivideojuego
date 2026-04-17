using UnityEngine;
using Firebase.Firestore; // Esta es la línea que resuelve tu error
using Firebase.Extensions;
using System.Collections.Generic;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseFirestore db;
    private string documentPath = "player1"; // Debe coincidir con tu consola de Firebase
    public int monedasActuales = 0; 

    void Start()
    {
        // Esto inicializa Firebase en Unity de forma segura
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

    // Esta función se llama desde tu script de la moneda
    public void SumarMoneda()
    {
        monedasActuales++;
        ActualizarMonedasEnNube(monedasActuales);
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
            }
        });
    }



// Función para marcar un nivel como completado (true)
public void GuardarNivelCompletado(string nombreNivel)
{
    if (db == null) return;

    DocumentReference docRef = db.Collection("players").Document(documentPath);
    
    // Creamos un diccionario con el nombre del nivel (ej: "nivel1") y el valor true
    Dictionary<string, object> actualizacionNivel = new Dictionary<string, object>
    {
        { nombreNivel, true }
    };

    docRef.UpdateAsync(actualizacionNivel).ContinueWithOnMainThread(task => {
        if (task.IsCompleted) 
        {
            Debug.Log("¡Progreso de " + nombreNivel + " guardado en la nube!");
        }
        else 
        {
            Debug.LogError("Error al guardar nivel: " + task.Exception);
        }
    });
}

}
